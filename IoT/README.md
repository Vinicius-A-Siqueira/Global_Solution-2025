Global Solution 2025 - Sistema de Monitoramento e Bem-Estar no Trabalho
<div align="center">
O Futuro do Trabalho: Saúde Mental, Bem-Estar e Produtividade

</div>

👥 Integrantes
Gabriel Camargo – RM557879

Kauan Felipe – RM557954

Vinicius Alves – RM551939

📋 Índice
Visão Geral

O Problema

Nossa Solução

Objetivos

Pontos Fortes

Arquitetura e Tecnologias

Funcionalidades Principais

Entregas do Projeto

Como Executar

API de IA e Visão Computacional

Endpoints

Estrutura do Projeto

Demonstração

Licença

🌟 Visão Geral
A Global Solution 2025 é uma plataforma completa focada em melhorar a experiência do colaborador no ambiente corporativo.
O sistema integra IoT, Visão Computacional, Machine Learning, aplicativo mobile e backend conectado a banco híbrido, criando uma solução moderna para análise de bem-estar e comportamento no trabalho.

O objetivo é proporcionar ambientes mais saudáveis, produtivos e seguros, usando tecnologia avançada para detectar sinais de estresse, baixa ergonomia e riscos à saúde mental.

🚨 O Problema
As empresas enfrentam desafios crescentes como:

Níveis elevados de estresse e burnout

Posturas inadequadas durante o trabalho

Quedas na produtividade decorrentes de má qualidade do ambiente

Falta de monitoramento contínuo e automático

Dificuldades em identificar colaboradores vulneráveis

Pouca visibilidade para líderes e RH

💡 Nossa Solução
Propomos um sistema de monitoramento inteligente capaz de:

✔ Detectar posturas incorretas
Utilizando uma rede de Visão Computacional que identifica problemas ergonômicos (pescoço curvado, ombros caídos, distância incorreta do corpo etc.).

✔ Analisar emoções em tempo real
Classificação facial com Deep Learning para detectar possíveis sinais de cansaço, estresse ou desatenção.

✔ Integrar sensores IoT
Monitoramento de temperatura, ruído, iluminação e padrões de movimento.

✔ Dashboard completo para gestores
Com gráficos, tabelas, insights e sugestões automáticas baseadas em IA.

✔ Aplicativo mobile
Recebimento de notificações de bem-estar e acompanhamento do histórico.

🎯 Objetivos
Melhorar a ergonomia e prevenir problemas de saúde

Reduzir estresse e burnout

Aumentar a produtividade e satisfação dos colaboradores

Automatizar alertas e insights via IA

Oferecer dados em tempo real para líderes e RH

🏆 Pontos Fortes
🔍 Visão Computacional baseada em OpenCV + Mediapipe

🤖 Modelo de Deep Learning treinado para classificação de postura

📱 Integração com mobile (Flutter)

🔗 Backend Node/Python com APIs REST

🗃 Banco híbrido (PostgreSQL + Firebase Firestore)

📡 Sensores IoT integrados via MQTT

🚀 Deploy automatizado (Railway, Firebase Hosting, Render)

🏗 Arquitetura e Tecnologias
Backend
Python (FastAPI) para IA

Node.js/Express para API principal

MQTT para IoT

Swagger para documentação

Frontend Mobile
Flutter

Firebase Authentication

Banco de Dados
PostgreSQL (dados estruturados)

Firestore (coletas rápidas e logs)

IA / Visão Computacional
OpenCV

Mediapipe

TensorFlow / Keras

Modelos pré-treinados MobileNet / BlazePose

IoT
ESP32 / ESP8266

Sensores DHT11, LDR, Microfone MEMS

🔧 Funcionalidades Principais
✔ Detecção de postura com IA

✔ Classificação de emoções

✔ Monitoramento ambiental IoT

✔ Alertas automáticos em tempo real

✔ Histórico de saúde e produtividade

✔ Dashboard interativo

✔ Relatórios automatizados

📦 Entregas do Projeto
✔ Código Fonte completo (Backend + IA + Mobile + IoT)

✔ API com documentação (Swagger)

✔ Deploy funcional da IA e do backend

✔ Vídeo demonstrativo

✔ README profissional

▶ Como Executar
1. Clonar o repositório
git clone https://github.com/seu-repositorio.git
cd seu-projeto
2. Criar ambiente virtual
python -m venv venv
source venv/bin/activate # Linux/Mac
venv\Scripts\activate    # Windows
3. Instalar dependências
pip install -r requirements.txt
4. Executar a API
uvicorn app:app --reload
🤖 API de IA e Visão Computacional
Modelo usado:
MobileNetV2 para classificação de postura

Mediapipe Holistic para keypoints

OpenCV para pré-processamento

A API recebe imagens e retorna:

{
  "posture": "encurvado",
  "confidence": 0.91,
  "recommendation": "Ajustar a posição da coluna e elevar o monitor."
}
🔌 Endpoints
POST /predict/posture
Envia uma imagem e recebe a classificação.

POST /predict/emotion
Classifica emoção facial.

GET /health
Checagem de status da API.

📂 Estrutura do Projeto
/IoT
  /models
  /notebooks
  /dataset
  /api
  |   app.py
  |   utils.py
  |   posture_model.h5
  /mobile
  /backend
README.md
requirements.txt
🎥 Demonstração
📌 Link do vídeo (YouTube): a ser adicionado
📌 Mostra: IA funcionando + Mobile + IoT + Dashboard

📄 Licença
Projeto desenvolvido exclusivamente para fins acadêmicos – FIAP 2025.