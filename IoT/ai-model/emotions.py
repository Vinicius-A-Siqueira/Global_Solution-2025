from fer import FER
from PIL import Image
import numpy as np
import io

detector = FER(mtcnn=True)

def detectar_emocao(imagem_bytes):
    image = Image.open(io.BytesIO(imagem_bytes)).convert("RGB")
    np_img = np.array(image)

    result = detector.detect_emotions(np_img)

    if not result:
        return None

    emotions = result[0]["emotions"]
    emot = max(emotions, key=emotions.get)

    return {"emocao": emot, "confianca": float(emotions[emot])}
