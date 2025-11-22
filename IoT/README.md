# Global Solution 2025 - Sistema de Monitoramento e Bem-Estar no Trabalho

::: {align="center"}
## **O Futuro do Trabalho: Saúde Mental, Bem-Estar e Produtividade**
:::

![image](https://github.com/user-attachments/assets/6335eded-1ce5-41f1-8fbd-7921804f3f67)

------------------------------------------------------------------------

## 👥 Integrantes

-   **Gabriel Camargo** -- RM557879\
-   **Kauan Felipe** -- RM557954\
-   **Vinicius Alves** -- RM551939

------------------------------------------------------------------------

# WellMind Vision API

API de Visão Computacional baseada em Deep Learning para detecção de
emoções, reconhecimento facial e monitoramento de bem-estar mental em
ambientes corporativos.

[![Python](https://img.shields.io/badge/Python-3.11+-blue.svg)](https://www.python.org/)
[![FastAPI](https://img.shields.io/badge/FastAPI-0.115.0-green.svg)](https://fastapi.tiangolo.com/)
[![MTCNN](https://img.shields.io/badge/MTCNN-1.0.0-orange.svg)](https://github.com/ipazc/mtcnn)

------------------------------------------------------------------------

## 📋 Sumário

-   Visão Geral
-   Modelos de Deep Learning
-   Requisitos Atendidos
-   Instalação Rápida
-   Uso da API
-   Endpoints
-   Tecnologias
-   Arquitetura
-   Testes
-   Performance
-   Referências
-   Contato
-   Licença

------------------------------------------------------------------------

## 🎯 Visão Geral

Sistema de visão computacional para monitorar o bem-estar emocional de
colaboradores através de análise facial com modelos de Deep Learning.

### Problemas Resolvidos

-   Monitoramento não invasivo de check-ins voluntários\
-   Detecção precoce de estresse e fadiga\
-   Recomendações preventivas baseadas em IA\
-   Dados objetivos para programas de bem-estar corporativo

------------------------------------------------------------------------

## 🧠 Modelos de Deep Learning

### 1. **MTCNN (Multi-task Cascaded Convolutional Networks)**

**Função:** detecção facial + landmarks\
**Características:** - Pré-treinado em WIDER FACE\
- Detecção facial em tempo real (200--500 ms CPU)\
- Landmarks: olhos, nariz, canto da boca\
- Score de confiança

### 2. **DeepFace**

**Função:** classificação de emoções (7 classes)\
**Modelos:** VGG-Face + FER

**Emoções classificadas:**\
Happy, Sad, Angry, Fear, Surprise, Disgust, Neutral

------------------------------------------------------------------------

## ✅ Requisitos Atendidos (FIAP)

  Requisito                    Implementação      Status
  ---------------------------- ------------------ --------
  API de Visão Computacional   FastAPI REST       ✅
  Reconhecimento Facial        MTCNN              ✅
  Análise de Emoções           DeepFace           ✅
  Classificação Multi-classe   7 emoções          ✅
  Modelos Pré-treinados        MTCNN + DeepFace   ✅
  Integração com App           React Native       ✅
  Documentação                 Swagger + README   ✅

------------------------------------------------------------------------

## 📦 Instalação Rápida

### Pré-requisitos

-   Python 3.11+
-   pip
-   2GB RAM (mínimo)

### Instalação

    python -m venv venv

Ativar ambiente:

**Windows**

    .env\Scriptsctivate

Instalar dependências:

    pip install fastapi uvicorn[standard] python-multipart
    pip install opencv-python-headless pillow numpy
    pip install mtcnn deepface

### Rodar a API

**Versão simplificada (MTCNN):**

    uvicorn main_simple:app --reload

**Versão completa (com DeepFace):**

    uvicorn main:app --reload

### Documentação

-   Swagger UI → http://localhost:8000/docs\
-   ReDoc → http://localhost:8000/redoc

------------------------------------------------------------------------

## 📡 Endpoints

### **GET /health**

Verifica status dos modelos.

``` json
{
  "status": "saudável",
  "models": {"mtcnn": "carregado"}
}
```

------------------------------------------------------------------------

### **POST /api/v1/vision/detect-face**

Detecta faces via MTCNN.

#### Exemplo de resposta:

``` json
{
  "faces_detected": 1,
  "face_locations": [{
    "bounding_box": {"x": 120, "y": 85, "width": 200, "height": 250},
    "confidence": 0.99
  }],
  "model_used": "MTCNN"
}
```

------------------------------------------------------------------------

### **POST /api/v1/vision/analyze-emotion**

Classifica emoções e gera recomendações.

``` json
{
  "primary_emotion": "happy",
  "emotion_scores": {
    "happy": 0.85,
    "neutral": 0.10,
    "sad": 0.02
  },
  "stress_level": "Low",
  "fatigue_detected": false
}
```

------------------------------------------------------------------------

## 💡 Exemplo de Uso

### Python

``` python
import requests

url = "http://localhost:8000/api/v1/vision/detect-face"

with open("selfie.jpg", "rb") as f:
    files = {"file": f}
    response = requests.post(url, files=files)
    print(response.json())
```

### cURL

    curl -X POST "http://localhost:8000/api/v1/vision/detect-face" -F "file=@foto.jpg"

------------------------------------------------------------------------

## 🛠️ Tecnologias

-   FastAPI\
-   Uvicorn\
-   Pydantic\
-   OpenCV\
-   NumPy\
-   MTCNN\
-   TensorFlow\
-   DeepFace

------------------------------------------------------------------------

## 🏗️ Arquitetura

Aplicativo Mobile → FastAPI Backend → MTCNN / DeepFace

------------------------------------------------------------------------

## 📞 Contato

-   Email: contato@wellmind.com

------------------------------------------------------------------------

## 📄 Licença

MIT License © 2025
