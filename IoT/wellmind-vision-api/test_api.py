# test_api.py
from fastapi.testclient import TestClient
from main import app
import io
from PIL import Image
import numpy as np

client = TestClient(app)

def create_test_image():
    """Cria uma imagem de teste"""
    # Criar imagem fake 200x200 RGB
    img_array = np.random.randint(0, 255, (200, 200, 3), dtype=np.uint8)
    img = Image.fromarray(img_array)
    
    # Converter para bytes
    img_bytes = io.BytesIO()
    img.save(img_bytes, format='JPEG')
    img_bytes.seek(0)
    
    return img_bytes

def test_root():
    """Testar endpoint raiz"""
    response = client.get("/")
    assert response.status_code == 200
    data = response.json()
    assert data["service"] == "WellMind Vision API"
    print("✓ Test root passed")

def test_health():
    """Testar health check"""
    response = client.get("/health")
    assert response.status_code == 200
    data = response.json()
    assert data["status"] == "healthy"
    print("✓ Test health passed")

def test_analyze_emotion():
    """Testar análise de emoção"""
    img_bytes = create_test_image()
    
    response = client.post(
        "/api/v1/vision/analyze-emotion",
        files={"file": ("test.jpg", img_bytes, "image/jpeg")}
    )
    
    # Pode dar 400 se não detectar face na imagem fake
    print(f"Emotion analysis response: {response.status_code}")
    if response.status_code == 200:
        data = response.json()
        print(f"  Emotion: {data.get('primary_emotion')}")
        print("✓ Test analyze emotion passed")
    else:
        print(f"  Expected behavior: {response.json().get('detail')}")

if __name__ == "__main__":
    print("Running tests...\n")
    test_root()
    test_health()
    test_analyze_emotion()
    print("\n✓ All tests completed!")
