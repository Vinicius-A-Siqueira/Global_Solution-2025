# main_simple.py - Versão Simplificada (Apenas MTCNN)
"""
WellMind Vision API - Computer Vision Simplified

Modelos de Deep Learning Utilizados:
1. MTCNN (Multi-task Cascaded Convolutional Networks) - Detecção facial

Esta versão simplificada foca em detecção facial robusta sem dependência
de TensorFlow pesado, ideal para desenvolvimento e testes rápidos.
"""

from fastapi import FastAPI, File, UploadFile, HTTPException, Header
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel, Field
from typing import Optional, List, Dict, Any
import cv2
import numpy as np
from datetime import datetime
import logging

# Configurar logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

# Inicializar FastAPI
app = FastAPI(
    title="WellMind Vision API - Simplified",
    description="""
    # Computer Vision API for Face Detection
    
    ## Deep Learning Model:
    - **MTCNN**: Multi-task Cascaded Convolutional Neural Networks
    
    ## Capabilities:
    - ✓ Face Detection
    - ✓ Facial Landmarks (5 points)
    - ✓ Confidence Scoring
    - ✓ Multi-face Detection
    - ✓ Basic Emotion Estimation (rule-based)
    
    Versão simplificada sem DeepFace para testes rápidos.
    """,
    version="1.0.0-simple"
)

# CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Carregar MTCNN
try:
    from mtcnn import MTCNN
    face_detector = MTCNN()
    logger.info("✓ MTCNN loaded successfully")
except Exception as e:
    logger.error(f"✗ Error loading MTCNN: {e}")
    face_detector = None

# Modelos Pydantic
class FaceDetectionResponse(BaseModel):
    faces_detected: int = Field(..., description="Número de faces detectadas")
    face_locations: List[Dict[str, Any]] = Field(..., description="Localização das faces")
    confidence: float = Field(..., description="Confiança média das detecções")
    timestamp: str = Field(..., description="Timestamp ISO 8601")
    model_used: str = Field(..., description="Modelo utilizado")

class SimpleEmotionResponse(BaseModel):
    detected_faces: int
    estimated_mood: str
    confidence: float
    stress_level: str
    recommendations: List[str]
    timestamp: str
    note: str

# Funções auxiliares
def decode_image(file_bytes: bytes) -> np.ndarray:
    """Decodifica bytes para imagem OpenCV"""
    try:
        nparr = np.frombuffer(file_bytes, np.uint8)
        img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        return img
    except Exception as e:
        logger.error(f"Error decoding image: {e}")
        return None

def estimate_mood_from_geometry(keypoints: dict, confidence: float) -> tuple:
    """
    Estimativa básica de humor baseada em geometria facial
    (Simplificado - não substitui análise real de emoções)
    """
    # Extrair pontos
    left_eye = keypoints['left_eye']
    right_eye = keypoints['right_eye']
    mouth_left = keypoints['mouth_left']
    mouth_right = keypoints['mouth_right']
    
    # Calcular ângulo da boca (simplificado)
    mouth_slope = (mouth_right[1] - mouth_left[1]) / (mouth_right[0] - mouth_left[0] + 1e-6)
    
    # Estimativa básica
    if abs(mouth_slope) < 0.05:
        mood = "neutral"
        stress = "Medium"
    elif mouth_slope < -0.05:
        mood = "positive"  # Boca "sorrindo"
        stress = "Low"
    else:
        mood = "negative"  # Boca "triste"
        stress = "High"
    
    # Ajustar por confiança
    if confidence < 0.7:
        stress = "Medium"
    
    return mood, stress

def generate_basic_recommendations(mood: str, stress: str) -> List[str]:
    """Recomendações baseadas em estimativa"""
    recommendations = []
    
    if mood == "positive":
        recommendations.append("✅ Humor positivo detectado - Continue assim!")
        recommendations.append("🌟 Compartilhe sua energia positiva com a equipe")
    elif mood == "negative":
        recommendations.append("⚠️ Considere fazer uma pausa de 10 minutos")
        recommendations.append("🧘 Pratique exercícios de respiração")
    else:
        recommendations.append("🌿 Mantenha o equilíbrio e autocuidado")
    
    if stress == "High":
        recommendations.append("💬 Fale com seu gestor sobre a carga de trabalho")
        recommendations.append("😴 Verifique sua qualidade de sono")
    
    return recommendations

async def verify_token(authorization: Optional[str] = Header(None)):
    """Auth simplificado"""
    return True

# ==================== ENDPOINTS ====================

@app.get("/", tags=["Root"])
async def root():
    """Endpoint raiz"""
    return {
        "service": "WellMind Vision API - Simplified",
        "version": "1.0.0-simple",
        "status": "operational",
        "description": "Versão simplificada com MTCNN apenas",
        "endpoints": {
            "health": "/health",
            "detect_face": "/api/v1/vision/detect-face",
            "analyze_simple": "/api/v1/vision/analyze-simple",
            "documentation": "/docs"
        }
    }

@app.get("/health", tags=["Health"])
async def health_check():
    """Health check"""
    return {
        "status": "healthy",
        "timestamp": datetime.utcnow().isoformat(),
        "models": {
            "mtcnn": "loaded" if face_detector else "error"
        },
        "note": "Versão simplificada - MTCNN apenas"
    }

@app.post("/api/v1/vision/detect-face", response_model=FaceDetectionResponse, tags=["Vision AI"])
async def detect_face(
    file: UploadFile = File(..., description="Imagem com face (JPEG, PNG)"),
    authenticated: bool = Depends(verify_token)
):
    """
    # Detecção Facial com MTCNN
    
    Detecta faces e retorna:
    - Localização (bounding box)
    - Pontos de referência (olhos, nariz, boca)
    - Confiança da detecção
    
    Modelo: MTCNN (Multi-task Cascaded CNN)
    """
    try:
        # Validar arquivo
        if not file.content_type.startswith('image/'):
            raise HTTPException(
                status_code=400,
                detail=f"Tipo de arquivo inválido: {file.content_type}"
            )
        
        # Ler imagem
        contents = await file.read()
        img = decode_image(contents)
        
        if img is None:
            raise HTTPException(status_code=400, detail="Imagem inválida")
        
        if face_detector is None:
            raise HTTPException(status_code=500, detail="Detector não disponível")
        
        # Detectar faces
        logger.info("Executando detecção MTCNN...")
        faces = face_detector.detect_faces(img)
        
        if not faces:
            raise HTTPException(
                status_code=400,
                detail="Nenhuma face detectada. Use uma foto com rosto visível."
            )
        
        # Processar faces detectadas
        face_locations = []
        confidences = []
        
        for idx, face in enumerate(faces):
            box = face['box']
            keypoints = face['keypoints']
            conf = face['confidence']
            confidences.append(conf)
            
            face_locations.append({
                "face_id": idx,
                "bounding_box": {
                    "x": int(box[0]),
                    "y": int(box[1]),
                    "width": int(box[2]),
                    "height": int(box[3])
                },
                "confidence": float(conf),
                "landmarks": {
                    "left_eye": [int(keypoints['left_eye'][0]), int(keypoints['left_eye'][1])],
                    "right_eye": [int(keypoints['right_eye'][0]), int(keypoints['right_eye'][1])],
                    "nose": [int(keypoints['nose'][0]), int(keypoints['nose'][1])],
                    "mouth_left": [int(keypoints['mouth_left'][0]), int(keypoints['mouth_left'][1])],
                    "mouth_right": [int(keypoints['mouth_right'][0]), int(keypoints['mouth_right'][1])]
                }
            })
        
        avg_confidence = sum(confidences) / len(confidences)
        
        logger.info(f"✓ {len(faces)} face(s) detectada(s) com confiança média: {avg_confidence:.2f}")
        
        return FaceDetectionResponse(
            faces_detected=len(faces),
            face_locations=face_locations,
            confidence=avg_confidence,
            timestamp=datetime.utcnow().isoformat(),
            model_used="MTCNN (Multi-task Cascaded CNN)"
        )
        
    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"Erro na detecção: {e}")
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/api/v1/vision/analyze-simple", response_model=SimpleEmotionResponse, tags=["Vision AI"])
async def analyze_simple(
    file: UploadFile = File(...),
    authenticated: bool = Depends(verify_token)
):
    """
    # Análise Simplificada de Humor
    
    Estimativa básica baseada em:
    - Geometria facial (MTCNN landmarks)
    - Confiança da detecção
    - Regras heurísticas
    
    **Nota**: Esta é uma versão simplificada. Para análise precisa
    de emoções, use a versão completa com DeepFace.
    """
    try:
        contents = await file.read()
        img = decode_image(contents)
        
        if img is None or face_detector is None:
            raise HTTPException(status_code=400, detail="Imagem ou detector inválido")
        
        faces = face_detector.detect_faces(img)
        
        if not faces:
            raise HTTPException(status_code=400, detail="Nenhuma face detectada")
        
        # Usar face com maior confiança
        face = max(faces, key=lambda x: x['confidence'])
        confidence = face['confidence']
        keypoints = face['keypoints']
        
        # Estimativa de humor (simplificada)
        mood, stress = estimate_mood_from_geometry(keypoints, confidence)
        
        # Gerar recomendações
        recommendations = generate_basic_recommendations(mood, stress)
        
        return SimpleEmotionResponse(
            detected_faces=len(faces),
            estimated_mood=mood,
            confidence=confidence,
            stress_level=stress,
            recommendations=recommendations,
            timestamp=datetime.utcnow().isoformat(),
            note="Estimativa baseada em geometria facial - Use versão completa para análise precisa"
        )
        
    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"Erro na análise: {e}")
        raise HTTPException(status_code=500, detail=str(e))

if __name__ == "__main__":
    import uvicorn
    logger.info("Iniciando WellMind Vision API (Simplified)...")
    uvicorn.run(app, host="0.0.0.0", port=8000)
