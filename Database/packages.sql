-- ============================================================================
-- PACKAGE 1: PKG_USUARIO
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_USUARIO AS

    PROCEDURE SP_INSERIR (
        p_nome IN VARCHAR2,
        p_email IN VARCHAR2,
        p_senha_hash IN VARCHAR2,
        p_data_nascimento IN DATE,
        p_genero IN VARCHAR2,
        p_telefone IN VARCHAR2,
        p_id_usuario OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_usuario IN NUMBER
    ) RETURN CLOB;
    
    FUNCTION FN_VALIDAR_EMAIL_CORPORATIVO (
        p_email IN VARCHAR2
    ) RETURN VARCHAR2;

END PKG_USUARIO;
/

CREATE OR REPLACE PACKAGE BODY PKG_USUARIO AS

    PROCEDURE SP_INSERIR (
        p_nome IN VARCHAR2,
        p_email IN VARCHAR2,
        p_senha_hash IN VARCHAR2,
        p_data_nascimento IN DATE,
        p_genero IN VARCHAR2,
        p_telefone IN VARCHAR2,
        p_id_usuario OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_email_existe NUMBER;
        v_idade NUMBER;
    BEGIN
        IF p_email IS NULL THEN
            RAISE_APPLICATION_ERROR(-20001, 'Email é obrigatório');
        END IF;

        SELECT COUNT(*) INTO v_email_existe FROM USUARIO WHERE email = p_email;
        IF v_email_existe > 0 THEN
            RAISE_APPLICATION_ERROR(-20002, 'Email já cadastrado');
        END IF;

        v_idade := TRUNC((SYSDATE - p_data_nascimento) / 365.25);
        IF v_idade < 16 THEN
            RAISE_APPLICATION_ERROR(-20003, 'Idade mínima: 16 anos');
        END IF;

        IF p_genero NOT IN ('Masculino', 'Feminino', 'Outro', 'Prefiro não informar') THEN
            RAISE_APPLICATION_ERROR(-20004, 'Gênero inválido');
        END IF;

        INSERT INTO USUARIO (id_usuario, nome, email, senha_hash, data_nascimento, genero, telefone, data_cadastro, status_ativo)
        VALUES (seq_usuario.NEXTVAL, p_nome, p_email, p_senha_hash, p_data_nascimento, p_genero, p_telefone, SYSDATE, 'S')
        RETURNING id_usuario INTO p_id_usuario;

        p_status := 'SUCESSO';
        p_mensagem := 'Usuário cadastrado com sucesso';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;
    
    FUNCTION FN_VALIDAR_EMAIL_CORPORATIVO (
        p_email IN VARCHAR2
    ) RETURN VARCHAR2 AS
        v_resultado VARCHAR2(4000);
        v_dominio VARCHAR2(200);
        v_regex_email VARCHAR2(200);
        v_regex_dominio VARCHAR2(200);
    BEGIN
        -- Verificação 1: Email não pode ser nulo
        IF p_email IS NULL THEN
            RETURN '{
              "valido": false,
              "mensagem": "Email é obrigatório",
              "tipo_erro": "email_vazio"
            }';
        END IF;

        -- Verificação 2: Email deve conter @
        IF INSTR(p_email, '@') = 0 THEN
            RETURN '{
              "valido": false,
              "mensagem": "Email deve conter símbolo @",
              "tipo_erro": "formato_invalido"
            }';
        END IF;

        -- Verificação 3: Validar formato com REGEXP
        -- Padrão: usuario@dominio.com.br ou usuario@empresa.com
        IF NOT REGEXP_LIKE(p_email, '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$') THEN
            RETURN '{
              "valido": false,
              "mensagem": "Formato de email inválido",
              "tipo_erro": "regex_falhou"
            }';
        END IF;

        -- Verificação 4: Não permitir emails de domínios pessoais comuns
        v_dominio := SUBSTR(p_email, INSTR(p_email, '@') + 1);
        IF v_dominio IN ('gmail.com', 'hotmail.com', 'yahoo.com', 'outlook.com') THEN
            RETURN '{
              "valido": true,
              "mensagem": "Email pessoal (não corporativo). Recomenda-se usar email da empresa.",
              "tipo_email": "pessoal",
              "dominio": "' || v_dominio || '"
            }';
        END IF;

        -- Email válido e corporativo
        v_resultado := '{' || CHR(10);
        v_resultado := v_resultado || '  "valido": true,' || CHR(10);
        v_resultado := v_resultado || '  "email": "' || p_email || '",' || CHR(10);
        v_resultado := v_resultado || '  "tipo_email": "corporativo",' || CHR(10);
        v_resultado := v_resultado || '  "dominio": "' || v_dominio || '",' || CHR(10);
        v_resultado := v_resultado || '  "mensagem": "Email corporativo validado com sucesso"' || CHR(10);
        v_resultado := v_resultado || '}';

        RETURN v_resultado;

    EXCEPTION
        WHEN OTHERS THEN
            RETURN '{
              "valido": false,
              "mensagem": "Erro ao validar email: ' || SQLERRM || '",
              "tipo_erro": "excecao_nao_tratada"
            }';
    END FN_VALIDAR_EMAIL_CORPORATIVO;

    FUNCTION FN_PARA_JSON (p_id_usuario IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec USUARIO%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM USUARIO WHERE id_usuario = p_id_usuario;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "nome": "' || REPLACE(v_rec.nome, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "email": "' || v_rec.email || '",' || CHR(10);
        v_json := v_json || '  "data_nascimento": "' || TO_CHAR(v_rec.data_nascimento, 'YYYY-MM-DD') || '",' || CHR(10);
        v_json := v_json || '  "genero": "' || v_rec.genero || '",' || CHR(10);
        v_json := v_json || '  "telefone": "' || COALESCE(v_rec.telefone, '') || '",' || CHR(10);
        v_json := v_json || '  "data_cadastro": "' || TO_CHAR(v_rec.data_cadastro, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json := v_json || '  "status_ativo": "' || v_rec.status_ativo || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Usuário não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_USUARIO;
/

-- ============================================================================
-- PACKAGE 2: PKG_EMPRESA
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_EMPRESA AS

    PROCEDURE SP_INSERIR (
        p_nome_empresa IN VARCHAR2,
        p_cnpj IN VARCHAR2,
        p_endereco IN VARCHAR2,
        p_telefone IN VARCHAR2,
        p_email_contato IN VARCHAR2,
        p_id_empresa OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_empresa IN NUMBER
    ) RETURN CLOB;

END PKG_EMPRESA;
/

CREATE OR REPLACE PACKAGE BODY PKG_EMPRESA AS

    PROCEDURE SP_INSERIR (
        p_nome_empresa IN VARCHAR2,
        p_cnpj IN VARCHAR2,
        p_endereco IN VARCHAR2,
        p_telefone IN VARCHAR2,
        p_email_contato IN VARCHAR2,
        p_id_empresa OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_cnpj_existe NUMBER;
    BEGIN
        IF p_cnpj IS NULL THEN
            RAISE_APPLICATION_ERROR(-20010, 'CNPJ é obrigatório');
        END IF;

        IF NOT REGEXP_LIKE(p_cnpj, '^[0-9]{2}\.[0-9]{3}\.[0-9]{3}/[0-9]{4}-[0-9]{2}$') THEN
            RAISE_APPLICATION_ERROR(-20011, 'CNPJ formato inválido');
        END IF;

        SELECT COUNT(*) INTO v_cnpj_existe FROM EMPRESA WHERE cnpj = p_cnpj;
        IF v_cnpj_existe > 0 THEN
            RAISE_APPLICATION_ERROR(-20012, 'CNPJ já cadastrado');
        END IF;

        INSERT INTO EMPRESA (id_empresa, nome_empresa, cnpj, endereco, telefone, email_contato, data_cadastro)
        VALUES (seq_empresa.NEXTVAL, p_nome_empresa, p_cnpj, p_endereco, p_telefone, p_email_contato, SYSDATE)
        RETURNING id_empresa INTO p_id_empresa;

        p_status := 'SUCESSO';
        p_mensagem := 'Empresa cadastrada';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_empresa IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec EMPRESA%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM EMPRESA WHERE id_empresa = p_id_empresa;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_empresa": ' || v_rec.id_empresa || ',' || CHR(10);
        v_json := v_json || '  "nome_empresa": "' || REPLACE(v_rec.nome_empresa, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "cnpj": "' || v_rec.cnpj || '",' || CHR(10);
        v_json := v_json || '  "endereco": "' || COALESCE(v_rec.endereco, '') || '",' || CHR(10);
        v_json := v_json || '  "telefone": "' || COALESCE(v_rec.telefone, '') || '",' || CHR(10);
        v_json := v_json || '  "email_contato": "' || COALESCE(v_rec.email_contato, '') || '",' || CHR(10);
        v_json := v_json || '  "data_cadastro": "' || TO_CHAR(v_rec.data_cadastro, 'YYYY-MM-DD') || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Empresa não encontrada"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_EMPRESA;
/

-- ============================================================================
-- PACKAGE 3: PKG_USUARIO_EMPRESA
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_USUARIO_EMPRESA AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_empresa IN NUMBER,
        p_cargo IN VARCHAR2,
        p_id_usuario_empresa OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_usuario_empresa IN NUMBER
    ) RETURN CLOB;

END PKG_USUARIO_EMPRESA;
/

CREATE OR REPLACE PACKAGE BODY PKG_USUARIO_EMPRESA AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_empresa IN NUMBER,
        p_cargo IN VARCHAR2,
        p_id_usuario_empresa OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_usuario_existe NUMBER;
        v_empresa_existe NUMBER;
        v_vinculo_existe NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_usuario_existe FROM USUARIO WHERE id_usuario = p_id_usuario AND status_ativo = 'S';
        IF v_usuario_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20020, 'Usuário não encontrado');
        END IF;

        SELECT COUNT(*) INTO v_empresa_existe FROM EMPRESA WHERE id_empresa = p_id_empresa;
        IF v_empresa_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20021, 'Empresa não encontrada');
        END IF;

        SELECT COUNT(*) INTO v_vinculo_existe FROM USUARIO_EMPRESA 
        WHERE id_usuario = p_id_usuario AND id_empresa = p_id_empresa AND status_vinculo = 'A';
        IF v_vinculo_existe > 0 THEN
            RAISE_APPLICATION_ERROR(-20022, 'Vínculo já existe');
        END IF;

        INSERT INTO USUARIO_EMPRESA (id_usuario_empresa, id_usuario, id_empresa, cargo, data_vinculo, status_vinculo)
        VALUES (seq_usuario_empresa.NEXTVAL, p_id_usuario, p_id_empresa, p_cargo, SYSDATE, 'A')
        RETURNING id_usuario_empresa INTO p_id_usuario_empresa;

        p_status := 'SUCESSO';
        p_mensagem := 'Vínculo criado';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_usuario_empresa IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec USUARIO_EMPRESA%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM USUARIO_EMPRESA WHERE id_usuario_empresa = p_id_usuario_empresa;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_usuario_empresa": ' || v_rec.id_usuario_empresa || ',' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "id_empresa": ' || v_rec.id_empresa || ',' || CHR(10);
        v_json := v_json || '  "cargo": "' || REPLACE(v_rec.cargo, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "data_vinculo": "' || TO_CHAR(v_rec.data_vinculo, 'YYYY-MM-DD') || '",' || CHR(10);
        v_json := v_json || '  "status_vinculo": "' || v_rec.status_vinculo || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Vínculo não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_USUARIO_EMPRESA;
/

-- ============================================================================
-- PACKAGE 4: PKG_REGISTRO_BEMESTAR
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_REGISTRO_BEMESTAR AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_nivel_humor IN NUMBER,
        p_nivel_estresse IN NUMBER,
        p_nivel_energia IN NUMBER,
        p_horas_sono IN NUMBER,
        p_qualidade_sono IN NUMBER,
        p_observacoes IN VARCHAR2,
        p_id_registro OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );
    
    FUNCTION FN_CALCULAR_INDICE_BEMESTAR (
        p_id_usuario IN NUMBER
    ) RETURN VARCHAR2;

    FUNCTION FN_PARA_JSON (
        p_id_registro IN NUMBER
    ) RETURN CLOB;

END PKG_REGISTRO_BEMESTAR;
/

CREATE OR REPLACE PACKAGE BODY PKG_REGISTRO_BEMESTAR AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_nivel_humor IN NUMBER,
        p_nivel_estresse IN NUMBER,
        p_nivel_energia IN NUMBER,
        p_horas_sono IN NUMBER,
        p_qualidade_sono IN NUMBER,
        p_observacoes IN VARCHAR2,
        p_id_registro OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_usuario_existe NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_usuario_existe FROM USUARIO WHERE id_usuario = p_id_usuario AND status_ativo = 'S';
        IF v_usuario_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20030, 'Usuário não encontrado');
        END IF;

        IF p_nivel_humor NOT BETWEEN 1 AND 10 THEN
            RAISE_APPLICATION_ERROR(-20031, 'Humor deve estar entre 1-10');
        END IF;

        IF p_nivel_estresse NOT BETWEEN 1 AND 10 THEN
            RAISE_APPLICATION_ERROR(-20032, 'Estresse deve estar entre 1-10');
        END IF;

        IF p_nivel_energia NOT BETWEEN 1 AND 10 THEN
            RAISE_APPLICATION_ERROR(-20033, 'Energia deve estar entre 1-10');
        END IF;

        IF p_horas_sono NOT BETWEEN 0 AND 24 THEN
            RAISE_APPLICATION_ERROR(-20034, 'Horas de sono entre 0-24');
        END IF;

        IF p_qualidade_sono NOT BETWEEN 1 AND 10 THEN
            RAISE_APPLICATION_ERROR(-20035, 'Qualidade sono entre 1-10');
        END IF;

        INSERT INTO REGISTRO_BEMESTAR (id_registro, id_usuario, data_registro, nivel_humor, nivel_estresse, 
                                       nivel_energia, horas_sono, qualidade_sono, observacoes)
        VALUES (seq_registro_bemestar.NEXTVAL, p_id_usuario, SYSTIMESTAMP, p_nivel_humor, p_nivel_estresse,
                p_nivel_energia, p_horas_sono, p_qualidade_sono, p_observacoes)
        RETURNING id_registro INTO p_id_registro;

        p_status := 'SUCESSO';
        p_mensagem := 'Registro inserido';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_registro IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec REGISTRO_BEMESTAR%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM REGISTRO_BEMESTAR WHERE id_registro = p_id_registro;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_registro": ' || v_rec.id_registro || ',' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "data_registro": "' || TO_CHAR(v_rec.data_registro, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json := v_json || '  "nivel_humor": ' || v_rec.nivel_humor || ',' || CHR(10);
        v_json := v_json || '  "nivel_estresse": ' || v_rec.nivel_estresse || ',' || CHR(10);
        v_json := v_json || '  "nivel_energia": ' || v_rec.nivel_energia || ',' || CHR(10);
        v_json := v_json || '  "horas_sono": ' || v_rec.horas_sono || ',' || CHR(10);
        v_json := v_json || '  "qualidade_sono": ' || v_rec.qualidade_sono || ',' || CHR(10);
        v_json := v_json || '  "observacoes": "' || COALESCE(REPLACE(v_rec.observacoes, '"', '\"'), '') || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Registro não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;
    
    FUNCTION FN_CALCULAR_INDICE_BEMESTAR (
        p_id_usuario IN NUMBER
    ) RETURN VARCHAR2 AS
        v_resultado VARCHAR2(4000);
        v_humor_medio NUMBER;
        v_estresse_medio NUMBER;
        v_energia_media NUMBER;
        v_sono_medio NUMBER;
        v_indice_bemestar NUMBER;
        v_status_saude VARCHAR2(50);
        v_alerta VARCHAR2(500);
        v_percentual_compatibilidade NUMBER;
        v_registro_count NUMBER;
    BEGIN
        -- EXCEÇÃO 1: Validar se usuário existe e tem registros
        BEGIN
            SELECT 
                COUNT(*),
                AVG(nivel_humor),
                AVG(nivel_estresse),
                AVG(nivel_energia),
                AVG(horas_sono)
            INTO v_registro_count, v_humor_medio, v_estresse_medio, v_energia_media, v_sono_medio
            FROM REGISTRO_BEMESTAR
            WHERE id_usuario = p_id_usuario;

            IF v_registro_count = 0 THEN
                RAISE_APPLICATION_ERROR(-20020, 'Usuário não tem registros de bem-estar suficientes');
            END IF;
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('[EXCEÇÃO 1] Erro ao calcular bem-estar do usuário ' || p_id_usuario);
                RETURN '{
                  "erro": "DADOS_INSUFICIENTES",
                  "mensagem": "Não há registros de bem-estar para este usuário",
                  "usuario_id": ' || p_id_usuario || ',
                  "tipo_excecao": 1
                }';
        END;

        -- EXCEÇÃO 2: Validar intervalos de valores com REGEXP
        BEGIN
            IF NOT (v_humor_medio BETWEEN 1 AND 10 AND
                    v_estresse_medio BETWEEN 1 AND 10 AND
                    v_energia_media BETWEEN 1 AND 10 AND
                    v_sono_medio BETWEEN 0 AND 24) THEN
                RAISE_APPLICATION_ERROR(-20021, 'Valores fora dos intervalos válidos');
            END IF;

            -- Padrão Regex: validar que os valores são números válidos
            -- Este é um exemplo prático de uso de REGEXP
            IF NOT REGEXP_LIKE(ROUND(v_humor_medio, 2), '^[0-9]+\.?[0-9]*$') THEN
                RAISE_APPLICATION_ERROR(-20022, 'Formato numérico inválido detectado');
            END IF;
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('[EXCEÇÃO 2] Erro de validação com REGEXP: ' || SQLERRM);
                RETURN '{
                  "erro": "VALIDACAO_FALHOU",
                  "mensagem": "Valores de bem-estar não passaram na validação",
                  "tipo_excecao": 2
                }';
        END;

        -- Cálculo do Índice de Bem-estar (fórmula: média ponderada)
        -- Humor: 40%, Energia: 40%, Estresse inverso: 20%
        v_indice_bemestar := (v_humor_medio * 0.4) + (v_energia_media * 0.4) + ((10 - v_estresse_medio) * 0.2);

        -- Definir status de saúde baseado no índice
        IF v_indice_bemestar >= 8 THEN
            v_status_saude := 'EXCELENTE';
            v_percentual_compatibilidade := 95;
        ELSIF v_indice_bemestar >= 6.5 THEN
            v_status_saude := 'BOM';
            v_percentual_compatibilidade := 75;
        ELSIF v_indice_bemestar >= 5 THEN
            v_status_saude := 'ALERTA';
            v_percentual_compatibilidade := 50;
        ELSE
            v_status_saude := 'CRÍTICO';
            v_percentual_compatibilidade := 25;
        END IF;

        -- Detectar riscos específicos
        IF v_estresse_medio >= 8 THEN
            v_alerta := 'CRÍTICO: Nível de estresse muito elevado (>8). Recomenda-se consulta imediata com profissional de saúde mental.';
        ELSIF v_humor_medio <= 3 THEN
            v_alerta := 'ATENÇÃO: Nível de humor muito baixo (<=3). Busque apoio profissional sem demora.';
        ELSIF v_energia_media <= 2 THEN
            v_alerta := 'ALERTA: Nível de energia crítico (<=2). Considere repouso ou avaliação médica urgente.';
        ELSIF v_sono_medio < 6 THEN
            v_alerta := 'AVISO: Sono insuficiente (<6h). Melhore a higiene do sono e qualidade de repouso.';
        ELSE
            v_alerta := 'POSITIVO: Seus indicadores estão dentro dos limites normais. Continue monitorando seu bem-estar!';
        END IF;

        -- CONSTRUÇÃO MANUAL DO JSON (sem usar TO_JSON, JSON_OBJECT, etc.)
        v_resultado := '{' || CHR(10);
        v_resultado := v_resultado || '  "analise_bemestar": {' || CHR(10);
        v_resultado := v_resultado || '    "usuario_id": ' || p_id_usuario || ',' || CHR(10);
        v_resultado := v_resultado || '    "total_registros": ' || v_registro_count || ',' || CHR(10);
        v_resultado := v_resultado || '    "indicadores": {' || CHR(10);
        v_resultado := v_resultado || '      "humor_medio": ' || ROUND(v_humor_medio, 2) || ',' || CHR(10);
        v_resultado := v_resultado || '      "estresse_medio": ' || ROUND(v_estresse_medio, 2) || ',' || CHR(10);
        v_resultado := v_resultado || '      "energia_media": ' || ROUND(v_energia_media, 2) || ',' || CHR(10);
        v_resultado := v_resultado || '      "sono_medio_horas": ' || ROUND(v_sono_medio, 2) || CHR(10);
        v_resultado := v_resultado || '    },' || CHR(10);
        v_resultado := v_resultado || '    "indice_bemestar": ' || ROUND(v_indice_bemestar, 2) || ',' || CHR(10);
        v_resultado := v_resultado || '    "status_saude": "' || v_status_saude || '",' || CHR(10);
        v_resultado := v_resultado || '    "compatibilidade_bem_estar": ' || v_percentual_compatibilidade || '%,' || CHR(10);
        v_resultado := v_resultado || '    "mensagem_alerta": "' || REPLACE(v_alerta, '"', '\"') || '",' || CHR(10);
        v_resultado := v_resultado || '    "recomendacao": "' || 
            CASE 
                WHEN v_status_saude = 'EXCELENTE' THEN 'Mantenha suas práticas de bem-estar. Você é um exemplo!'
                WHEN v_status_saude = 'BOM' THEN 'Continue monitorando. Pequenas melhorias podem trazer grandes benefícios.'
                WHEN v_status_saude = 'ALERTA' THEN 'Procure implementar atividades de bem-estar imediatamente.'
                ELSE 'URGENTE: Busque apoio profissional. Sua saúde mental é prioridade.'
            END || '",' || CHR(10);
        v_resultado := v_resultado || '    "timestamp": "' || TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS') || '"' || CHR(10);
        v_resultado := v_resultado || '  }' || CHR(10);
        v_resultado := v_resultado || '}';

        DBMS_OUTPUT.PUT_LINE('[LOG CALCULAR] Bem-estar calculado para usuário ' || p_id_usuario || ' - Status: ' || v_status_saude);

        RETURN v_resultado;

    EXCEPTION
        WHEN OTHERS THEN
            -- EXCEÇÃO 3: Erro genérico não previsto
            DBMS_OUTPUT.PUT_LINE('[EXCEÇÃO 3] Erro em FN_CALCULAR_INDICE_BEMESTAR: ' || SQLERRM);
            RETURN '{
              "erro": "ERRO_INTERNO",
              "mensagem": "Erro ao calcular índice de bem-estar: ' || REPLACE(SQLERRM, '"', '\"') || '",
              "tipo_excecao": 3
            }';
    END FN_CALCULAR_INDICE_BEMESTAR;

END PKG_REGISTRO_BEMESTAR;
/

-- ============================================================================
-- PACKAGE 5: PKG_CATEGORIA_RECOMENDACAO
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_CATEGORIA_RECOMENDACAO AS

    PROCEDURE SP_INSERIR (
        p_nome_categoria IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_id_categoria OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_categoria IN NUMBER
    ) RETURN CLOB;

END PKG_CATEGORIA_RECOMENDACAO;
/

CREATE OR REPLACE PACKAGE BODY PKG_CATEGORIA_RECOMENDACAO AS

    PROCEDURE SP_INSERIR (
        p_nome_categoria IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_id_categoria OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_existe NUMBER;
    BEGIN
        IF p_nome_categoria IS NULL THEN
            RAISE_APPLICATION_ERROR(-20040, 'Nome categoria obrigatório');
        END IF;

        SELECT COUNT(*) INTO v_existe FROM CATEGORIA_RECOMENDACAO WHERE nome_categoria = p_nome_categoria;
        IF v_existe > 0 THEN
            RAISE_APPLICATION_ERROR(-20041, 'Categoria já existe');
        END IF;

        INSERT INTO CATEGORIA_RECOMENDACAO (id_categoria, nome_categoria, descricao)
        VALUES (seq_categoria_recomendacao.NEXTVAL, p_nome_categoria, p_descricao)
        RETURNING id_categoria INTO p_id_categoria;

        p_status := 'SUCESSO';
        p_mensagem := 'Categoria criada';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_categoria IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec CATEGORIA_RECOMENDACAO%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM CATEGORIA_RECOMENDACAO WHERE id_categoria = p_id_categoria;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_categoria": ' || v_rec.id_categoria || ',' || CHR(10);
        v_json := v_json || '  "nome_categoria": "' || REPLACE(v_rec.nome_categoria, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "descricao": "' || COALESCE(REPLACE(v_rec.descricao, '"', '\"'), '') || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Categoria não encontrada"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_CATEGORIA_RECOMENDACAO;
/

-- ============================================================================
-- PACKAGE 6: PKG_RECOMENDACAO
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_RECOMENDACAO AS

    PROCEDURE SP_INSERIR (
        p_id_categoria IN NUMBER,
        p_titulo IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_tipo_recomendacao IN VARCHAR2,
        p_conteudo IN CLOB,
        p_id_recomendacao OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_recomendacao IN NUMBER
    ) RETURN CLOB;

END PKG_RECOMENDACAO;
/

CREATE OR REPLACE PACKAGE BODY PKG_RECOMENDACAO AS

    PROCEDURE SP_INSERIR (
        p_id_categoria IN NUMBER,
        p_titulo IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_tipo_recomendacao IN VARCHAR2,
        p_conteudo IN CLOB,
        p_id_recomendacao OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_categoria_existe NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_categoria_existe FROM CATEGORIA_RECOMENDACAO WHERE id_categoria = p_id_categoria;
        IF v_categoria_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20050, 'Categoria não encontrada');
        END IF;

        IF p_tipo_recomendacao NOT IN ('texto', 'video', 'audio', 'exercicio', 'artigo') THEN
            RAISE_APPLICATION_ERROR(-20051, 'Tipo inválido');
        END IF;

        INSERT INTO RECOMENDACAO (id_recomendacao, id_categoria, titulo, descricao, tipo_recomendacao, conteudo)
        VALUES (seq_recomendacao.NEXTVAL, p_id_categoria, p_titulo, p_descricao, p_tipo_recomendacao, p_conteudo)
        RETURNING id_recomendacao INTO p_id_recomendacao;

        p_status := 'SUCESSO';
        p_mensagem := 'Recomendação criada';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_recomendacao IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec RECOMENDACAO%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM RECOMENDACAO WHERE id_recomendacao = p_id_recomendacao;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_recomendacao": ' || v_rec.id_recomendacao || ',' || CHR(10);
        v_json := v_json || '  "id_categoria": ' || v_rec.id_categoria || ',' || CHR(10);
        v_json := v_json || '  "titulo": "' || REPLACE(v_rec.titulo, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "descricao": "' || COALESCE(REPLACE(v_rec.descricao, '"', '\"'), '') || '",' || CHR(10);
        v_json := v_json || '  "tipo_recomendacao": "' || v_rec.tipo_recomendacao || '",' || CHR(10);
        v_json := v_json || '  "conteudo": "' || COALESCE(REPLACE(SUBSTR(v_rec.conteudo, 1, 500), '"', '\"'), '') || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Recomendação não encontrada"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_RECOMENDACAO;
/

-- ============================================================================
-- PACKAGE 7: PKG_RECOMENDACAO_USUARIO
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_RECOMENDACAO_USUARIO AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_recomendacao IN NUMBER,
        p_status_visualizacao IN CHAR DEFAULT 'N',
        p_avaliacao_usuario IN NUMBER DEFAULT NULL,
        p_id_recomendacao_usuario OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_recomendacao_usuario IN NUMBER
    ) RETURN CLOB;

END PKG_RECOMENDACAO_USUARIO;
/

CREATE OR REPLACE PACKAGE BODY PKG_RECOMENDACAO_USUARIO AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_recomendacao IN NUMBER,
        p_status_visualizacao IN CHAR DEFAULT 'N',
        p_avaliacao_usuario IN NUMBER DEFAULT NULL,
        p_id_recomendacao_usuario OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_usuario_existe NUMBER;
        v_recomendacao_existe NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_usuario_existe FROM USUARIO WHERE id_usuario = p_id_usuario;
        IF v_usuario_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20060, 'Usuário não encontrado');
        END IF;

        SELECT COUNT(*) INTO v_recomendacao_existe FROM RECOMENDACAO WHERE id_recomendacao = p_id_recomendacao;
        IF v_recomendacao_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20061, 'Recomendação não encontrada');
        END IF;

        INSERT INTO RECOMENDACAO_USUARIO (id_recomendacao_usuario, id_usuario, id_recomendacao, 
                                          data_recomendacao, status_visualizacao, avaliacao_usuario)
        VALUES (seq_recomendacao_usuario.NEXTVAL, p_id_usuario, p_id_recomendacao, 
                SYSTIMESTAMP, p_status_visualizacao, p_avaliacao_usuario)
        RETURNING id_recomendacao_usuario INTO p_id_recomendacao_usuario;

        p_status := 'SUCESSO';
        p_mensagem := 'Recomendação enviada';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_recomendacao_usuario IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec RECOMENDACAO_USUARIO%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM RECOMENDACAO_USUARIO WHERE id_recomendacao_usuario = p_id_recomendacao_usuario;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_recomendacao_usuario": ' || v_rec.id_recomendacao_usuario || ',' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "id_recomendacao": ' || v_rec.id_recomendacao || ',' || CHR(10);
        v_json := v_json || '  "data_recomendacao": "' || TO_CHAR(v_rec.data_recomendacao, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json := v_json || '  "status_visualizacao": "' || v_rec.status_visualizacao || '",' || CHR(10);
        v_json := v_json || '  "avaliacao_usuario": ' || COALESCE(TO_CHAR(v_rec.avaliacao_usuario), 'null') || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Registro não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_RECOMENDACAO_USUARIO;
/

-- ============================================================================
-- PACKAGE 8: PKG_ALERTA
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_ALERTA AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_tipo_alerta IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_nivel_gravidade IN VARCHAR2,
        p_status_alerta IN VARCHAR2 DEFAULT 'PENDENTE',
        p_id_alerta OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_alerta IN NUMBER
    ) RETURN CLOB;

END PKG_ALERTA;
/

CREATE OR REPLACE PACKAGE BODY PKG_ALERTA AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_tipo_alerta IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_nivel_gravidade IN VARCHAR2,
        p_status_alerta IN VARCHAR2 DEFAULT 'PENDENTE',
        p_id_alerta OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_usuario_existe NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_usuario_existe FROM USUARIO WHERE id_usuario = p_id_usuario;
        IF v_usuario_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20070, 'Usuário não encontrado');
        END IF;

        IF p_nivel_gravidade NOT IN ('BAIXO', 'MEDIO', 'ALTO', 'CRITICO') THEN
            RAISE_APPLICATION_ERROR(-20071, 'Nível gravidade inválido');
        END IF;

        IF p_status_alerta NOT IN ('PENDENTE', 'EM_ANALISE', 'RESOLVIDO', 'IGNORADO') THEN
            RAISE_APPLICATION_ERROR(-20072, 'Status inválido');
        END IF;

        INSERT INTO ALERTA (id_alerta, id_usuario, tipo_alerta, descricao, data_alerta, nivel_gravidade, status_alerta)
        VALUES (seq_alerta.NEXTVAL, p_id_usuario, p_tipo_alerta, p_descricao, SYSTIMESTAMP, p_nivel_gravidade, p_status_alerta)
        RETURNING id_alerta INTO p_id_alerta;

        p_status := 'SUCESSO';
        p_mensagem := 'Alerta criado';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_alerta IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec ALERTA%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM ALERTA WHERE id_alerta = p_id_alerta;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_alerta": ' || v_rec.id_alerta || ',' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "tipo_alerta": "' || REPLACE(v_rec.tipo_alerta, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "descricao": "' || COALESCE(REPLACE(v_rec.descricao, '"', '\"'), '') || '",' || CHR(10);
        v_json := v_json || '  "data_alerta": "' || TO_CHAR(v_rec.data_alerta, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json := v_json || '  "nivel_gravidade": "' || v_rec.nivel_gravidade || '",' || CHR(10);
        v_json := v_json || '  "status_alerta": "' || v_rec.status_alerta || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Alerta não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_ALERTA;
/

-- ============================================================================
-- PACKAGE 9: PKG_CONQUISTA
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_CONQUISTA AS

    PROCEDURE SP_INSERIR (
        p_nome_conquista IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_icone IN VARCHAR2,
        p_pontos IN NUMBER DEFAULT 0,
        p_id_conquista OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_conquista IN NUMBER
    ) RETURN CLOB;

END PKG_CONQUISTA;
/

CREATE OR REPLACE PACKAGE BODY PKG_CONQUISTA AS

    PROCEDURE SP_INSERIR (
        p_nome_conquista IN VARCHAR2,
        p_descricao IN VARCHAR2,
        p_icone IN VARCHAR2,
        p_pontos IN NUMBER DEFAULT 0,
        p_id_conquista OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_existe NUMBER;
    BEGIN
        IF p_nome_conquista IS NULL THEN
            RAISE_APPLICATION_ERROR(-20080, 'Nome conquista obrigatório');
        END IF;

        SELECT COUNT(*) INTO v_existe FROM CONQUISTA WHERE nome_conquista = p_nome_conquista;
        IF v_existe > 0 THEN
            RAISE_APPLICATION_ERROR(-20081, 'Conquista já existe');
        END IF;

        IF p_pontos < 0 THEN
            RAISE_APPLICATION_ERROR(-20082, 'Pontos devem ser >= 0');
        END IF;

        INSERT INTO CONQUISTA (id_conquista, nome_conquista, descricao, icone, pontos)
        VALUES (seq_conquista.NEXTVAL, p_nome_conquista, p_descricao, p_icone, p_pontos)
        RETURNING id_conquista INTO p_id_conquista;

        p_status := 'SUCESSO';
        p_mensagem := 'Conquista criada';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_conquista IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec CONQUISTA%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM CONQUISTA WHERE id_conquista = p_id_conquista;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_conquista": ' || v_rec.id_conquista || ',' || CHR(10);
        v_json := v_json || '  "nome_conquista": "' || REPLACE(v_rec.nome_conquista, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "descricao": "' || COALESCE(REPLACE(v_rec.descricao, '"', '\"'), '') || '",' || CHR(10);
        v_json := v_json || '  "icone": "' || COALESCE(v_rec.icone, '') || '",' || CHR(10);
        v_json := v_json || '  "pontos": ' || v_rec.pontos || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Conquista não encontrada"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_CONQUISTA;
/

-- ============================================================================
-- PACKAGE 10: PKG_USUARIO_CONQUISTA
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_USUARIO_CONQUISTA AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_conquista IN NUMBER,
        p_pontos_ganhos IN NUMBER DEFAULT 0,
        p_id_usuario_conquista OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_usuario_conquista IN NUMBER
    ) RETURN CLOB;

END PKG_USUARIO_CONQUISTA;
/

CREATE OR REPLACE PACKAGE BODY PKG_USUARIO_CONQUISTA AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_conquista IN NUMBER,
        p_pontos_ganhos IN NUMBER DEFAULT 0,
        p_id_usuario_conquista OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_usuario_existe NUMBER;
        v_conquista_existe NUMBER;
        v_ja_conquistou NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_usuario_existe FROM USUARIO WHERE id_usuario = p_id_usuario;
        IF v_usuario_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20090, 'Usuário não encontrado');
        END IF;

        SELECT COUNT(*) INTO v_conquista_existe FROM CONQUISTA WHERE id_conquista = p_id_conquista;
        IF v_conquista_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20091, 'Conquista não encontrada');
        END IF;

        SELECT COUNT(*) INTO v_ja_conquistou FROM USUARIO_CONQUISTA 
        WHERE id_usuario = p_id_usuario AND id_conquista = p_id_conquista;
        IF v_ja_conquistou > 0 THEN
            RAISE_APPLICATION_ERROR(-20092, 'Usuário já tem esta conquista');
        END IF;

        INSERT INTO USUARIO_CONQUISTA (id_usuario_conquista, id_usuario, id_conquista, data_conquista, pontos_ganhos)
        VALUES (seq_usuario_conquista.NEXTVAL, p_id_usuario, p_id_conquista, SYSTIMESTAMP, p_pontos_ganhos)
        RETURNING id_usuario_conquista INTO p_id_usuario_conquista;

        p_status := 'SUCESSO';
        p_mensagem := 'Conquista atribuída';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_usuario_conquista IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec USUARIO_CONQUISTA%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM USUARIO_CONQUISTA WHERE id_usuario_conquista = p_id_usuario_conquista;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_usuario_conquista": ' || v_rec.id_usuario_conquista || ',' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "id_conquista": ' || v_rec.id_conquista || ',' || CHR(10);
        v_json := v_json || '  "data_conquista": "' || TO_CHAR(v_rec.data_conquista, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json := v_json || '  "pontos_ganhos": ' || v_rec.pontos_ganhos || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Registro não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_USUARIO_CONQUISTA;
/

-- ============================================================================
-- PACKAGE 11: PKG_PROFISSIONAL_SAUDE
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_PROFISSIONAL_SAUDE AS

    PROCEDURE SP_INSERIR (
        p_nome IN VARCHAR2,
        p_especialidade IN VARCHAR2,
        p_crp_crm IN VARCHAR2,
        p_email IN VARCHAR2,
        p_telefone IN VARCHAR2,
        p_disponivel IN CHAR DEFAULT 'S',
        p_id_profissional OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_profissional IN NUMBER
    ) RETURN CLOB;

END PKG_PROFISSIONAL_SAUDE;
/

CREATE OR REPLACE PACKAGE BODY PKG_PROFISSIONAL_SAUDE AS

    PROCEDURE SP_INSERIR (
        p_nome IN VARCHAR2,
        p_especialidade IN VARCHAR2,
        p_crp_crm IN VARCHAR2,
        p_email IN VARCHAR2,
        p_telefone IN VARCHAR2,
        p_disponivel IN CHAR DEFAULT 'S',
        p_id_profissional OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_existe NUMBER;
    BEGIN
        IF p_nome IS NULL OR p_especialidade IS NULL OR p_crp_crm IS NULL THEN
            RAISE_APPLICATION_ERROR(-20100, 'Campos obrigatórios não preenchidos');
        END IF;

        SELECT COUNT(*) INTO v_existe FROM PROFISSIONAL_SAUDE WHERE crp_crm = p_crp_crm;
        IF v_existe > 0 THEN
            RAISE_APPLICATION_ERROR(-20101, 'CRP/CRM já cadastrado');
        END IF;

        IF p_disponivel NOT IN ('S', 'N') THEN
            RAISE_APPLICATION_ERROR(-20102, 'Disponível deve ser S ou N');
        END IF;

        INSERT INTO PROFISSIONAL_SAUDE (id_profissional, nome, especialidade, crp_crm, email, telefone, disponivel)
        VALUES (seq_profissional_saude.NEXTVAL, p_nome, p_especialidade, p_crp_crm, p_email, p_telefone, p_disponivel)
        RETURNING id_profissional INTO p_id_profissional;

        p_status := 'SUCESSO';
        p_mensagem := 'Profissional cadastrado';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_profissional IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec PROFISSIONAL_SAUDE%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM PROFISSIONAL_SAUDE WHERE id_profissional = p_id_profissional;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_profissional": ' || v_rec.id_profissional || ',' || CHR(10);
        v_json := v_json || '  "nome": "' || REPLACE(v_rec.nome, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "especialidade": "' || REPLACE(v_rec.especialidade, '"', '\"') || '",' || CHR(10);
        v_json := v_json || '  "crp_crm": "' || v_rec.crp_crm || '",' || CHR(10);
        v_json := v_json || '  "email": "' || COALESCE(v_rec.email, '') || '",' || CHR(10);
        v_json := v_json || '  "telefone": "' || COALESCE(v_rec.telefone, '') || '",' || CHR(10);
        v_json := v_json || '  "disponivel": "' || v_rec.disponivel || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Profissional não encontrado"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_PROFISSIONAL_SAUDE;
/

-- ============================================================================
-- PACKAGE 12: PKG_SESSAO_APOIO
-- ============================================================================

CREATE OR REPLACE PACKAGE PKG_SESSAO_APOIO AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_profissional IN NUMBER,
        p_data_sessao IN TIMESTAMP,
        p_duracao_minutos IN NUMBER,
        p_tipo_sessao IN VARCHAR2,
        p_observacoes IN VARCHAR2,
        p_id_sessao OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    );

    FUNCTION FN_PARA_JSON (
        p_id_sessao IN NUMBER
    ) RETURN CLOB;

END PKG_SESSAO_APOIO;
/

CREATE OR REPLACE PACKAGE BODY PKG_SESSAO_APOIO AS

    PROCEDURE SP_INSERIR (
        p_id_usuario IN NUMBER,
        p_id_profissional IN NUMBER,
        p_data_sessao IN TIMESTAMP,
        p_duracao_minutos IN NUMBER,
        p_tipo_sessao IN VARCHAR2,
        p_observacoes IN VARCHAR2,
        p_id_sessao OUT NUMBER,
        p_status OUT VARCHAR2,
        p_mensagem OUT VARCHAR2
    ) AS
        v_usuario_existe NUMBER;
        v_profissional_existe NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_usuario_existe FROM USUARIO WHERE id_usuario = p_id_usuario;
        IF v_usuario_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20110, 'Usuário não encontrado');
        END IF;

        SELECT COUNT(*) INTO v_profissional_existe FROM PROFISSIONAL_SAUDE WHERE id_profissional = p_id_profissional;
        IF v_profissional_existe = 0 THEN
            RAISE_APPLICATION_ERROR(-20111, 'Profissional não encontrado');
        END IF;

        IF p_duracao_minutos <= 0 THEN
            RAISE_APPLICATION_ERROR(-20112, 'Duração deve ser > 0');
        END IF;

        IF p_tipo_sessao NOT IN ('individual', 'grupo', 'emergencia', 'acompanhamento') THEN
            RAISE_APPLICATION_ERROR(-20113, 'Tipo sessão inválido');
        END IF;

        INSERT INTO SESSAO_APOIO (id_sessao, id_usuario, id_profissional, data_sessao, duracao_minutos, tipo_sessao, observacoes)
        VALUES (seq_sessao_apoio.NEXTVAL, p_id_usuario, p_id_profissional, p_data_sessao, p_duracao_minutos, p_tipo_sessao, p_observacoes)
        RETURNING id_sessao INTO p_id_sessao;

        p_status := 'SUCESSO';
        p_mensagem := 'Sessão agendada';
        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            p_status := 'ERRO';
            p_mensagem := SQLERRM;
            ROLLBACK;
    END SP_INSERIR;

    FUNCTION FN_PARA_JSON (p_id_sessao IN NUMBER) RETURN CLOB AS
        v_json CLOB;
        v_rec SESSAO_APOIO%ROWTYPE;
    BEGIN
        SELECT * INTO v_rec FROM SESSAO_APOIO WHERE id_sessao = p_id_sessao;

        v_json := '{' || CHR(10);
        v_json := v_json || '  "id_sessao": ' || v_rec.id_sessao || ',' || CHR(10);
        v_json := v_json || '  "id_usuario": ' || v_rec.id_usuario || ',' || CHR(10);
        v_json := v_json || '  "id_profissional": ' || v_rec.id_profissional || ',' || CHR(10);
        v_json := v_json || '  "data_sessao": "' || TO_CHAR(v_rec.data_sessao, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json := v_json || '  "duracao_minutos": ' || v_rec.duracao_minutos || ',' || CHR(10);
        v_json := v_json || '  "tipo_sessao": "' || v_rec.tipo_sessao || '",' || CHR(10);
        v_json := v_json || '  "observacoes": "' || COALESCE(REPLACE(v_rec.observacoes, '"', '\"'), '') || '"' || CHR(10);
        v_json := v_json || '}';

        RETURN v_json;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '{"erro": "Sessão não encontrada"}';
        WHEN OTHERS THEN
            RETURN '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
    END FN_PARA_JSON;

END PKG_SESSAO_APOIO;
/

COMMIT;

