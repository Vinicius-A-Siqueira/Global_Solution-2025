from fastapi import FastAPI, UploadFile, File
from ai_model.emotions import detectar_emocao

app = FastAPI()

# ROTA 1 — Receber foto e classificar emoção
@app.post("/api/emocao")
async def emocao(file: UploadFile = File(...)):
    bytes_img = await file.read()
    resultado = detectar_emocao(bytes_img)

    if resultado is None:
        return {"erro": "Nenhum rosto detectado"}

    return resultado


# ROTA 2 — Receber dados do IoT
@app.post("/api/iot")
async def receber_iot(payload: dict):
    # Você pode salvar no banco, mas para o vídeo só mostrar na tela já basta
    return {"status": "OK", "recebido": payload}
