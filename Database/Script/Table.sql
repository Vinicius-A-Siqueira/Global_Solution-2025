CREATE TABLE USUARIO (
    id_usuario NUMBER PRIMARY KEY,
    nome VARCHAR2(100) NOT NULL,
    email VARCHAR2(100) NOT NULL UNIQUE,
    senha_hash VARCHAR2(255) NOT NULL,
    data_nascimento DATE NOT NULL,
    genero VARCHAR2(20),
    telefone VARCHAR2(20),
    data_cadastro DATE DEFAULT SYSDATE NOT NULL,
    status_ativo CHAR(1) DEFAULT 'S' NOT NULL,
    CONSTRAINT ck_usuario_status CHECK (status_ativo IN ('S', 'N')),
    CONSTRAINT ck_usuario_genero CHECK (genero IN ('Masculino', 'Feminino', 'Outro', 'Prefiro não informar'))
);

-- ============================================================================
-- TABELA: EMPRESA
-- Armazena informações das empresas/organizações
-- ============================================================================

CREATE TABLE EMPRESA (
    id_empresa NUMBER PRIMARY KEY,
    nome_empresa VARCHAR2(200) NOT NULL,
    cnpj VARCHAR2(18) NOT NULL UNIQUE,
    endereco VARCHAR2(300),
    telefone VARCHAR2(20),
    email_contato VARCHAR2(100),
    data_cadastro DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT ck_cnpj_formato CHECK (REGEXP_LIKE(cnpj, '^[0-9]{2}\.[0-9]{3}\.[0-9]{3}/[0-9]{4}-[0-9]{2}$'))
);

-- ============================================================================
-- TABELA: USUARIO_EMPRESA (Tabela Associativa)
-- Relacionamento N:N entre Usuários e Empresas
-- ============================================================================

CREATE TABLE USUARIO_EMPRESA (
    id_usuario_empresa NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    id_empresa NUMBER NOT NULL,
    cargo VARCHAR2(100),
    data_vinculo DATE DEFAULT SYSDATE NOT NULL,
    status_vinculo CHAR(1) DEFAULT 'A' NOT NULL,
    CONSTRAINT fk_usuario_empresa_usuario FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_usuario_empresa_empresa FOREIGN KEY (id_empresa) REFERENCES EMPRESA(id_empresa) ON DELETE CASCADE,
    CONSTRAINT ck_usuario_empresa_status CHECK (status_vinculo IN ('A', 'I')),
    CONSTRAINT uk_usuario_empresa UNIQUE (id_usuario, id_empresa)
);

-- ============================================================================
-- TABELA: REGISTRO_BEMESTAR
-- Armazena os registros diários de bem-estar dos usuários
-- ============================================================================

CREATE TABLE REGISTRO_BEMESTAR (
    id_registro NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    data_registro TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,
    nivel_humor NUMBER(2) NOT NULL,
    nivel_estresse NUMBER(2) NOT NULL,
    nivel_energia NUMBER(2) NOT NULL,
    horas_sono NUMBER(4,2),
    qualidade_sono NUMBER(2),
    observacoes VARCHAR2(500),
    CONSTRAINT fk_registro_usuario FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario) ON DELETE CASCADE,
    CONSTRAINT ck_nivel_humor CHECK (nivel_humor BETWEEN 1 AND 10),
    CONSTRAINT ck_nivel_estresse CHECK (nivel_estresse BETWEEN 1 AND 10),
    CONSTRAINT ck_nivel_energia CHECK (nivel_energia BETWEEN 1 AND 10),
    CONSTRAINT ck_horas_sono CHECK (horas_sono >= 0 AND horas_sono <= 24),
    CONSTRAINT ck_qualidade_sono CHECK (qualidade_sono BETWEEN 1 AND 10)
);

-- ============================================================================
-- TABELA: CATEGORIA_RECOMENDACAO
-- Categorias para organizar recomendações
-- ============================================================================

CREATE TABLE CATEGORIA_RECOMENDACAO (
    id_categoria NUMBER PRIMARY KEY,
    nome_categoria VARCHAR2(100) NOT NULL UNIQUE,
    descricao VARCHAR2(300)
);

-- ============================================================================
-- TABELA: RECOMENDACAO
-- Armazena as recomendações de bem-estar
-- ============================================================================

CREATE TABLE RECOMENDACAO (
    id_recomendacao NUMBER PRIMARY KEY,
    id_categoria NUMBER NOT NULL,
    titulo VARCHAR2(200) NOT NULL,
    descricao VARCHAR2(1000),
    tipo_recomendacao VARCHAR2(50) NOT NULL,
    conteudo CLOB,
    CONSTRAINT fk_recomendacao_categoria FOREIGN KEY (id_categoria) REFERENCES CATEGORIA_RECOMENDACAO(id_categoria) ON DELETE CASCADE,
    CONSTRAINT ck_tipo_recomendacao CHECK (tipo_recomendacao IN ('texto', 'video', 'audio', 'exercicio', 'artigo'))
);

-- ============================================================================
-- TABELA: RECOMENDACAO_USUARIO (Tabela Associativa)
-- Relacionamento N:N entre Recomendações e Usuários
-- ============================================================================

CREATE TABLE RECOMENDACAO_USUARIO (
    id_recomendacao_usuario NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    id_recomendacao NUMBER NOT NULL,
    data_recomendacao TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,
    status_visualizacao CHAR(1) DEFAULT 'N' NOT NULL,
    avaliacao_usuario NUMBER(1),
    CONSTRAINT fk_recom_usuario_usuario FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_recom_usuario_recom FOREIGN KEY (id_recomendacao) REFERENCES RECOMENDACAO(id_recomendacao) ON DELETE CASCADE,
    CONSTRAINT ck_status_visualizacao CHECK (status_visualizacao IN ('S', 'N')),
    CONSTRAINT ck_avaliacao_usuario CHECK (avaliacao_usuario BETWEEN 1 AND 5)
);

-- ============================================================================
-- TABELA: ALERTA
-- Armazena alertas de risco relacionados ao bem-estar
-- ============================================================================

CREATE TABLE ALERTA (
    id_alerta NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    tipo_alerta VARCHAR2(100) NOT NULL,
    descricao VARCHAR2(500),
    data_alerta TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,
    nivel_gravidade VARCHAR2(20) NOT NULL,
    status_alerta VARCHAR2(20) DEFAULT 'PENDENTE' NOT NULL,
    CONSTRAINT fk_alerta_usuario FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario) ON DELETE CASCADE,
    CONSTRAINT ck_nivel_gravidade CHECK (nivel_gravidade IN ('BAIXO', 'MEDIO', 'ALTO', 'CRITICO')),
    CONSTRAINT ck_status_alerta CHECK (status_alerta IN ('PENDENTE', 'EM_ANALISE', 'RESOLVIDO', 'IGNORADO'))
);

-- ============================================================================
-- TABELA: CONQUISTA
-- Armazena as conquistas disponíveis (gamificação)
-- ============================================================================

CREATE TABLE CONQUISTA (
    id_conquista NUMBER PRIMARY KEY,
    nome_conquista VARCHAR2(100) NOT NULL UNIQUE,
    descricao VARCHAR2(300),
    icone VARCHAR2(100),
    pontos NUMBER DEFAULT 0 NOT NULL,
    CONSTRAINT ck_pontos_conquista CHECK (pontos >= 0)
);

-- ============================================================================
-- TABELA: USUARIO_CONQUISTA (Tabela Associativa)
-- Relacionamento N:N entre Usuários e Conquistas
-- ============================================================================

CREATE TABLE USUARIO_CONQUISTA (
    id_usuario_conquista NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    id_conquista NUMBER NOT NULL,
    data_conquista TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,
    pontos_ganhos NUMBER DEFAULT 0 NOT NULL,
    CONSTRAINT fk_usuario_conquista_usuario FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_usuario_conquista_conquista FOREIGN KEY (id_conquista) REFERENCES CONQUISTA(id_conquista) ON DELETE CASCADE,
    CONSTRAINT uk_usuario_conquista UNIQUE (id_usuario, id_conquista),
    CONSTRAINT ck_pontos_ganhos CHECK (pontos_ganhos >= 0)
);

-- ============================================================================
-- TABELA: PROFISSIONAL_SAUDE
-- Armazena informações de profissionais de saúde mental
-- ============================================================================

CREATE TABLE PROFISSIONAL_SAUDE (
    id_profissional NUMBER PRIMARY KEY,
    nome VARCHAR2(100) NOT NULL,
    especialidade VARCHAR2(100) NOT NULL,
    crp_crm VARCHAR2(20) NOT NULL UNIQUE,
    email VARCHAR2(100),
    telefone VARCHAR2(20),
    disponivel CHAR(1) DEFAULT 'S' NOT NULL,
    CONSTRAINT ck_profissional_disponivel CHECK (disponivel IN ('S', 'N'))
);

-- ============================================================================
-- TABELA: SESSAO_APOIO
-- Armazena sessões de apoio psicológico
-- ============================================================================

CREATE TABLE SESSAO_APOIO (
    id_sessao NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    id_profissional NUMBER NOT NULL,
    data_sessao TIMESTAMP NOT NULL,
    duracao_minutos NUMBER NOT NULL,
    tipo_sessao VARCHAR2(50) NOT NULL,
    observacoes VARCHAR2(1000),
    CONSTRAINT fk_sessao_usuario FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_sessao_profissional FOREIGN KEY (id_profissional) REFERENCES PROFISSIONAL_SAUDE(id_profissional) ON DELETE CASCADE,
    CONSTRAINT ck_duracao_minutos CHECK (duracao_minutos > 0),
    CONSTRAINT ck_tipo_sessao CHECK (tipo_sessao IN ('individual', 'grupo', 'emergencia', 'acompanhamento'))
);

CREATE SEQUENCE seq_usuario START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_empresa START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_usuario_empresa START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_registro_bemestar START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_categoria_recomendacao START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_recomendacao START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_recomendacao_usuario START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_alerta START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_conquista START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_usuario_conquista START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_sessao_apoio START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_profissional_saude START WITH 1 INCREMENT BY 1;