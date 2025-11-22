# setup.ps1 - Script de configuração automatizada
Write-Host "🚀 Configurando WellMind Vision API..." -ForegroundColor Green

# Verificar se Python está instalado
if (-not (Get-Command python -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Python não encontrado. Instale Python 3.11+" -ForegroundColor Red
    exit 1
}

Write-Host "✓ Python encontrado: $(python --version)" -ForegroundColor Green

# Criar ambiente virtual se não existir
if (-not (Test-Path "venv")) {
    Write-Host "📦 Criando ambiente virtual..." -ForegroundColor Yellow
    python -m venv venv
}

# Ativar ambiente virtual
Write-Host "🔧 Ativando ambiente virtual..." -ForegroundColor Yellow
.\venv\Scripts\Activate.ps1

# Atualizar pip
Write-Host "⬆️  Atualizando pip..." -ForegroundColor Yellow
python -m pip install --upgrade pip

# Instalar dependências
Write-Host "📥 Instalando dependências..." -ForegroundColor Yellow
pip install -r requirements.txt

# Criar estrutura de diretórios
Write-Host "📁 Criando estrutura de arquivos..." -ForegroundColor Yellow
$directories = @("tests", "models", "utils", "docs")
foreach ($dir in $directories) {
    if (-not (Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir | Out-Null
    }
}

# Criar arquivo .env se não existir
if (-not (Test-Path ".env")) {
    Write-Host "📝 Criando arquivo .env..." -ForegroundColor Yellow
    Copy-Item .env.example .env
}

Write-Host ""
Write-Host "✅ Setup concluído com sucesso!" -ForegroundColor Green
Write-Host ""
Write-Host "Próximos passos:" -ForegroundColor Cyan
Write-Host "1. Edite o arquivo .env com suas credenciais"
Write-Host "2. Execute: uvicorn main:app --reload"
Write-Host "3. Acesse: http://localhost:8000/docs"
Write-Host ""
