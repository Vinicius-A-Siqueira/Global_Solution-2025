# Global Solution 2025 - Sistema de Monitoramento e Bem-Estar no Trabalho

<div align="center">

**O Futuro do Trabalho: Saúde Mental, Bem-Estar e Produtividade**

</div>

![image](https://github.com/user-attachments/assets/6335eded-1ce5-41f1-8fbd-7921804f3f67)

## 👥 Integrantes

- **Gabriel Camargo** – RM557879  
- **Kauan Felipe** – RM557954  
- **Vinicius Alves** – RM551939  


---

## 🏗️ Arquitetura

WellMind API segue o padrão **Clean Architecture** com Domain-Driven Design (DDD):

```
WellMindApi/
├── src/
│   ├── WellMindApi.Domain/              # Camada de Domínio (Entidades, Interfaces)
│   ├── WellMindApi.Application/         # Camada de Aplicação (Use Cases, DTOs, Validações)
│   ├── WellMindApi.Infrastructure/      # Camada de Infraestrutura (DB, Repositórios, Serviços Externos)
│   └── WellMindApi.Api/                 # Camada de API (Controllers, Middlewares, Configuração)
└── tests/
    ├── WellMindApi.UnitTests/           # Testes Unitários
    └── WellMindApi.IntegrationTests/    # Testes de Integração
```

### 📦 Camadas Explicadas

| Camada | Responsabilidade | Exemplos |
|--------|-----------------|----------|
| **Domain** | Lógica de negócio pura, entidades, regras | `Usuario`, `RegistroBemEstar`, `Alerta` |
| **Application** | Orquestração, use cases, DTOs, validações | `CriarUsuarioUseCase`, `AnalisarBemEstarUseCase` |
| **Infrastructure** | Persistência, acesso a dados, serviços externos | `UsuarioRepository`, `OracleHealthCheck`, ML.NET |
| **API** | Controllers, rotas, middlewares, Swagger | `UsuariosController`, `ErrorHandlingMiddleware` |

---

## 🔧 Tecnologias Utilizadas

- **.NET 9** com C# 13
- **Entity Framework Core 8** (ORM)
- **Oracle Database** (Banco de Dados)
- **ML.NET 3.0** (Previsão de Burnout)
- **JWT** (Autenticação)
- **Serilog** (Logging Estruturado)
- **FluentValidation** (Validação de Dados)
- **AutoMapper** (Mapeamento de Objetos)
- **Swagger/OpenAPI** (Documentação de API)
- **Health Checks** (Monitoramento de Saúde)

---

## 📋 Pré-Requisitos

- **Microsoft .NET 9 SDK** ou superior
- **Visual Studio 2022** (recomendado) ou **VS Code**
- **Oracle Database 12c+** (local ou remoto)
- **Git** para controle de versão

### Verificar Instalação do .NET

```bash
dotnet --version
```

---

## 🚀 Instalação e Configuração

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/WellMindApi.git
cd WellMindApi
```

### 2️⃣ Restaurar Dependências

```bash
dotnet restore
```

### 3️⃣ Configurar Banco de Dados

#### Opção A: Usar Oracle Local/Remoto

Edite `src/WellMindApi.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "WellMindDatabase": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)))"
  },
  "Jwt": {
    "SecretKey": "Sua-Chave-Secreta-Super-Segura-Aqui",
    "Issuer": "WellMindApi",
    "Audience": "WellMindClients",
    "ExpirationHours": 1
  }
}
```

#### Opção B: Criar Schema no Oracle

Conecte-se ao Oracle e execute:

```sql
-- Criar sequências
CREATE SEQUENCE SEQ_USUARIO START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE SEQ_EMPRESA START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE SEQ_REGISTRO_BEMESTAR START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE SEQ_ALERTA START WITH 1 INCREMENT BY 1;
-- ... outras sequências conforme necessário

-- Executar migrations (via EF Core)
```

### 4️⃣ Aplicar Migrations (Entity Framework Core)

```bash
cd src/WellMindApi.Api

# Aplicar migrations automaticamente (ao iniciar a app)
# OU manualmente:
dotnet ef database update --project ../WellMindApi.Infrastructure
```

### 5️⃣ Compilar o Projeto

```bash
dotnet build
```

Se houver erros de "arquivo em uso", feche o Visual Studio e todos os terminais com `dotnet run`, execute `dotnet clean` e tente novamente.

---

## ▶️ Executar a Aplicação

### Iniciar o Servidor de Desenvolvimento

```bash
cd src/WellMindApi.Api
dotnet run
```

O servidor iniciará em:
- **HTTP**: http://localhost:5100
- **HTTPS**: https://localhost:7216

Você deverá ver no console:
```
Now listening on: http://localhost:5100
Now listening on: https://localhost:7216
Application started. Press Ctrl+C to shut down.
```

### Acessar Swagger (Documentação Interativa)

Abra no navegador:
- [http://localhost:5100/swagger](http://localhost:5100/swagger)
- [https://localhost:7216/swagger](https://localhost:7216/swagger)

### Acessar Health UI (Dashboard de Monitoramento)

- [http://localhost:5100/health-ui](http://localhost:5100/health-ui)
- [https://localhost:7216/health-ui](https://localhost:7216/health-ui)

---

## 🔑 Endpoints Principais

### Autenticação

#### Login
```http
POST /api/v1/usuarios/login
Content-Type: application/json

{
  "email": "usuario@example.com",
  "senha": "senha123"
}

Response 200:
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "usuario": {
    "idUsuario": 1,
    "nome": "João Silva",
    "email": "usuario@example.com"
  }
}
```

### Usuários

#### Criar Usuário
```http
POST /api/v1/usuarios
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123",
  "dataNascimento": "1990-05-15",
  "genero": "M",
  "telefone": "11999999999"
}
```

#### Obter Usuário
```http
GET /api/v1/usuarios/{id}
Authorization: Bearer {token}
```

#### Listar Usuários (Paginado)
```http
GET /api/v1/usuarios?pageNumber=1&pageSize=10
Authorization: Bearer {token}
```

### Registros de Bem-Estar

#### Criar Registro
```http
POST /api/v1/registros
Authorization: Bearer {token}
Content-Type: application/json

{
  "idUsuario": 1,
  "nivelHumor": 8,
  "nivelEstresse": 4,
  "nivelEnergia": 7,
  "horasSono": 7.5,
  "qualidadeSono": 8
}
```

#### Obter Análise de Bem-Estar
```http
GET /api/v1/registros/usuario/{idUsuario}/analise
Authorization: Bearer {token}
```

Retorna análise completa com:
- Índice geral de bem-estar
- Status de saúde
- Risco de burnout
- Áreas de atenção
- Recomendações personalizadas

### Alertas

#### Listar Alertas
```http
GET /api/v1/alertas?idUsuario=1&status=PENDENTE
Authorization: Bearer {token}
```

#### Resolver Alerta
```http
PATCH /api/v1/alertas/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "RESOLVIDO",
  "acaoTomada": "Sessão com psicólogo agendada"
}
```

---

## 🔐 Autenticação JWT

Todos os endpoints (exceto `/login` e `/usuarios` POST) requerem token JWT.

### Como Usar

1. **Realizar Login** para obter o token
2. **Incluir o token** nos headers das próximas requisições:
   ```
   Authorization: Bearer <seu_token_aqui>
   ```

O token expira conforme configurado em `appsettings.json` (padrão: 1 hora em produção).

---

## 🏥 Health Checks

O sistema monitora continuamente a saúde da aplicação:

### Endpoints

- `GET /health` - Verifica todos os health checks
- `GET /health/ready` - Readiness (pronto para receber requisições)
- `GET /health/live` - Liveness (está funcionando)
- `GET /health-ui` - Dashboard visual interativo

### Checks Implementados

| Check | Descrição |
|-------|-----------|
| **oracle_database** | Conexão com banco Oracle |
| **memory** | Uso de memória da aplicação |

**Nota**: Se `oracle_database` retornar "Unhealthy" com erro `ORA-28000: account locked`, o usuário Oracle está bloqueado. Desbloqueie via DBA.

---

## 🤖 Inteligência Artificial - Predição de Burnout

WellMind usa **ML.NET** para prever risco de burnout analisando:

- Histórico de bem-estar (últimos 7 dias)
- Padrões de estresse e energia
- Qualidade do sono
- Mudanças de humor

### Como Funciona

1. Sistema coleta dados via registros de bem-estar
2. Modelo ML.NET processa os dados
3. Gera score de risco (0-100)
4. Se risco ≥ 75, cria alerta crítico automático
5. Envia recomendações baseadas em IA

---

## 📊 Estrutura de Dados

### Entidades Principais

#### Usuario
```csharp
- IdUsuario (PK)
- Nome
- Email (Unique)
- SenhaHash (BCrypt)
- DataNascimento
- Genero
- Telefone
- DataCadastro
- Ativo (bool)
```

#### RegistroBemEstar
```csharp
- IdRegistro (PK)
- IdUsuario (FK)
- NivelHumor (1-10)
- NivelEstresse (1-10)
- NivelEnergia (1-10)
- HorasSono (decimal)
- QualidadeSono (1-10)
- DataRegistro
```

#### Alerta
```csharp
- IdAlerta (PK)
- IdUsuario (FK)
- TipoAlerta (string)
- NivelGravidade (BAIXO, MEDIO, ALTO, CRITICO)
- Status (PENDENTE, EM_ANALISE, RESOLVIDO, CANCELADO)
- Descricao
- DataAlerta
```

---

## 🧪 Testes

### Executar Testes Unitários

```bash
dotnet test
```

### Executar Testes com Coverage

```bash
dotnet test /p:CollectCoverageMetrics=true
```

---

## 📝 Logging

WellMind usa **Serilog** para logging estruturado.

### Arquivos de Log

Os logs são salvos em:
```
logs/wellmind-{data}.log
```

### Níveis de Log

| Nível | Descrição |
|-------|-----------|
| **Debug** | Informações detalhadas para diagnóstico |
| **Information** | Fluxo geral da aplicação |
| **Warning** | Situações incomuns (Oracle locked, etc.) |
| **Error** | Erros recuperáveis |
| **Fatal** | Erros críticos que travam a aplicação |

---

## 🐛 Troubleshooting

### Erro: "The account is locked"

**Problema**: Usuário Oracle bloqueado por múltiplas tentativas de login.

**Solução**:
```sql
-- Conecte como admin no Oracle
ALTER USER SEU_USUARIO ACCOUNT UNLOCK;
ALTER USER SEU_USUARIO IDENTIFIED BY NOVA_SENHA;
```

### Erro: "arquivo em uso" ao compilar

**Problema**: DLLs travadas por processos antigos.

**Solução**:
```bash
# Feche Visual Studio e todos os terminais
dotnet clean
dotnet build
```

### API não inicia / porta não abre

**Problema**: Outro processo usando a mesma porta ou erro crítico.

**Solução**:
```bash
# Verificar processos usando portas
netstat -ano | findstr :5100
netstat -ano | findstr :7216

# Matar processo (Windows - substitua PID)
taskkill /PID 12345 /F

# Tentar novamente
dotnet run
```

### Swagger não carrega

**Problema**: Cache do navegador ou conexão SSL recusada.

**Solução**:
- Limpar cache do navegador (Ctrl+Shift+Delete)
- Aceitar risco do certificado auto-assinado em HTTPS
- Usar HTTP em desenvolvimento
- Tentar em outro navegador (Chrome, Firefox, Edge)

---

## 📚 Padrões de Projeto

WellMind implementa:

- **DDD (Domain-Driven Design)**: Foco na lógica de negócio
- **Clean Architecture**: Camadas bem definidas e independentes
- **Repository Pattern**: Abstração da persistência de dados
- **Unit of Work Pattern**: Transações coordenadas
- **Use Case Pattern**: Orquestração de operações complexas
- **DTO Pattern**: Transferência segura de dados
- **Specification Pattern**: Consultas reutilizáveis
- **Dependency Injection**: Loosely coupled components

---

## 🔄 Fluxo de Requisição

```
Cliente HTTP
    ↓
[API Gateway / Swagger UI]
    ↓
Middleware de Logging
    ↓
Middleware de Autenticação (JWT)
    ↓
Controller (ex: UsuariosController)
    ↓
Use Case (ex: CriarUsuarioUseCase)
    ↓
Validador (FluentValidation)
    ↓
Domain Service / Entity
    ↓
Repository (EF Core)
    ↓
Oracle Database
    ↓
Response JSON + Links HATEOAS
```

---

## 🚀 Deployment

### Preparar para Produção

1. **Usar arquivo de configuração `.env` ou secrets**:
   ```bash
   dotnet user-secrets set "ConnectionStrings:WellMindDatabase" "User Id=prod_user;..."
   dotnet user-secrets set "Jwt:SecretKey" "chave-super-segura-aqui"
   ```

2. **Build para Release**:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

3. **Usar Docker** (opcional):
   ```dockerfile
   FROM mcr.microsoft.com/dotnet/aspnet:9.0
   COPY publish/ .
   ENTRYPOINT ["dotnet", "WellMindApi.Api.dll"]
   ```

4. **Configurar HTTPS em Produção**: Use certificado válido (não auto-assinado)

---

## 📞 Suporte e Contribuição

### Como Contribuir

1. Fork o repositório
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Reportar Bugs

Abra uma issue no GitHub descrevendo:
- Versão do .NET e SO
- Passos para reproduzir
- Mensagem de erro completa
- Screenshots/logs

---

## 🗺️ Roadmap Futuro

- [ ] Autenticação Multi-Tenancy (isolamento por empresa)
- [ ] Notificações em Tempo Real (SignalR)
- [ ] Dashboard Web Interativo (React/Vue)
- [ ] Aplicativo Mobile (MAUI)
- [ ] Relatórios Avançados (PDF, Excel)
- [ ] Integração com Calendários (Google Calendar, Outlook)
- [ ] APIs de Terceiros (Slack, Teams)
- [ ] Machine Learning Mais Avançado (TensorFlow.NET)

---

## 📄 Licença

Este projeto está sob licença **MIT License** - veja o arquivo `LICENSE` para detalhes.

---

## 👥 Autores

**Desenvolvimento**: Vinicius Siqueira (FIAP - Turma 2025)

**Disciplina**: Global Solution - Saúde Mental Corporativa

---

## 📞 Contato

Para dúvidas ou sugestões, abra uma issue no repositório ou entre em contato via:
- Email: vinicius@fiap.com.br
- GitHub: [@ViniciusSiqueira](https://github.com/ViniciusSiqueira)

---

**Desenvolvido com ❤️ durante o programa Global Solution 2025 da FIAP**
