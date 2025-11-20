-- ============================================================================
-- PROCEDURE PARA EXPORTAR DATASET COMPLETO EM JSON PARA MONGODB
-- ============================================================================

CREATE OR REPLACE PROCEDURE SP_EXPORTAR_DATASET_MONGODB (
    p_dataset OUT CLOB
) AS
    v_json_usuarios CLOB;
    v_json_registros CLOB;
    v_json_recomendacoes CLOB;
    v_json_profissionais CLOB;
    v_json_alertas CLOB;
    v_json_sessoes CLOB;
    v_json_completo CLOB;

    v_usuario_rec USUARIO%ROWTYPE;
    v_registro_rec REGISTRO_BEMESTAR%ROWTYPE;
    v_recomendacao_rec RECOMENDACAO%ROWTYPE;
    v_profissional_rec PROFISSIONAL_SAUDE%ROWTYPE;
    v_alerta_rec ALERTA%ROWTYPE;
    v_sessao_rec SESSAO_APOIO%ROWTYPE;
    v_categoria_recomendacao CATEGORIA_RECOMENDACAO%ROWTYPE;

    v_primeira_usuario BOOLEAN := TRUE;
    v_primeira_registro BOOLEAN := TRUE;
    v_primeira_recomendacao BOOLEAN := TRUE;
    v_primeira_profissional BOOLEAN := TRUE;
    v_primeira_alerta BOOLEAN := TRUE;
    v_primeira_sessao BOOLEAN := TRUE;

    v_count_usuarios NUMBER := 0;
    v_count_registros NUMBER := 0;
    v_count_recomendacoes NUMBER := 0;
    v_count_profissionais NUMBER := 0;
    v_count_alertas NUMBER := 0;
    v_count_sessoes NUMBER := 0;

    v_humor_medio NUMBER;
    v_estresse_medio NUMBER;
    v_energia_media NUMBER;
    v_sono_medio NUMBER;
    v_count_bem_estar NUMBER;

    CURSOR c_usuarios IS SELECT * FROM USUARIO ORDER BY id_usuario;
    CURSOR c_registros IS SELECT * FROM REGISTRO_BEMESTAR ORDER BY data_registro DESC;
    CURSOR c_recomendacoes IS SELECT * FROM RECOMENDACAO ORDER BY id_recomendacao;
    CURSOR c_profissionais IS SELECT * FROM PROFISSIONAL_SAUDE ORDER BY id_profissional;
    CURSOR c_alertas IS SELECT * FROM ALERTA ORDER BY id_alerta;
    CURSOR c_sessoes IS SELECT * FROM SESSAO_APOIO ORDER BY id_sessao;

BEGIN
    DBMS_OUTPUT.PUT_LINE('Iniciando exportação de dataset para MongoDB...');

    -- ========================================================================
    -- SEÇÃO 1: EXPORTAR USUÁRIOS COM SEUS DADOS RELACIONADOS
    -- ========================================================================
    v_json_usuarios := '[' || CHR(10);

    FOR v_usuario_rec IN c_usuarios LOOP
        IF NOT v_primeira_usuario THEN
            v_json_usuarios := v_json_usuarios || ',' || CHR(10);
        END IF;

        v_count_usuarios := v_count_usuarios + 1;

        -- Calcular bem-estar para este usuário
        BEGIN
            SELECT COUNT(*), 
                   ROUND(AVG(nivel_humor), 2), 
                   ROUND(AVG(nivel_estresse), 2),
                   ROUND(AVG(nivel_energia), 2), 
                   ROUND(AVG(horas_sono), 2)
            INTO v_count_bem_estar, v_humor_medio, v_estresse_medio, v_energia_media, v_sono_medio
            FROM REGISTRO_BEMESTAR 
            WHERE id_usuario = v_usuario_rec.id_usuario;
        EXCEPTION WHEN OTHERS THEN
            v_count_bem_estar := 0;
            v_humor_medio := 0;
            v_estresse_medio := 0;
            v_energia_media := 0;
            v_sono_medio := 0;
        END;

        v_json_usuarios := v_json_usuarios || '  {' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "_id": "usuario_' || v_usuario_rec.id_usuario || '",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "tipo": "usuario",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "id_usuario": ' || v_usuario_rec.id_usuario || ',' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "nome": "' || REPLACE(v_usuario_rec.nome, '"', '\"') || '",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "email": "' || v_usuario_rec.email || '",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "idade": ' || TRUNC((SYSDATE - v_usuario_rec.data_nascimento) / 365.25) || ',' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "genero": "' || v_usuario_rec.genero || '",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "telefone": "' || COALESCE(v_usuario_rec.telefone, '') || '",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "status_ativo": "' || v_usuario_rec.status_ativo || '",' || CHR(10);
        v_json_usuarios := v_json_usuarios || '    "data_cadastro": "' || TO_CHAR(v_usuario_rec.data_cadastro, 'YYYY-MM-DD') || '",' || CHR(10);

        v_json_usuarios := v_json_usuarios || '    "bem_estar": {' || CHR(10);
        v_json_usuarios := v_json_usuarios || '      "total_registros": ' || v_count_bem_estar || ',' || CHR(10);
        v_json_usuarios := v_json_usuarios || '      "humor_medio": ' || COALESCE(v_humor_medio, 0) || ',' || CHR(10);
        v_json_usuarios := v_json_usuarios || '      "estresse_medio": ' || COALESCE(v_estresse_medio, 0) || ',' || CHR(10);
        v_json_usuarios := v_json_usuarios || '      "energia_media": ' || COALESCE(v_energia_media, 0) || ',' || CHR(10);
        v_json_usuarios := v_json_usuarios || '      "sono_medio": ' || COALESCE(v_sono_medio, 0) || CHR(10);
        v_json_usuarios := v_json_usuarios || '    }' || CHR(10);
        v_json_usuarios := v_json_usuarios || '  }';

        v_primeira_usuario := FALSE;
    END LOOP;

    v_json_usuarios := v_json_usuarios || CHR(10) || ']';

    -- ========================================================================
    -- SEÇÃO 2: EXPORTAR REGISTROS DE BEM-ESTAR (Time Series)
    -- ========================================================================
    v_json_registros := '[' || CHR(10);

    FOR v_registro_rec IN c_registros LOOP
        IF NOT v_primeira_registro THEN
            v_json_registros := v_json_registros || ',' || CHR(10);
        END IF;

        v_count_registros := v_count_registros + 1;

        v_json_registros := v_json_registros || '  {' || CHR(10);
        v_json_registros := v_json_registros || '    "_id": "registro_' || v_registro_rec.id_registro || '",' || CHR(10);
        v_json_registros := v_json_registros || '    "tipo": "registro_bemestar",' || CHR(10);
        v_json_registros := v_json_registros || '    "id_usuario": ' || v_registro_rec.id_usuario || ',' || CHR(10);
        v_json_registros := v_json_registros || '    "timestamp": "' || TO_CHAR(v_registro_rec.data_registro, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json_registros := v_json_registros || '    "indicadores": {' || CHR(10);
        v_json_registros := v_json_registros || '      "humor": ' || v_registro_rec.nivel_humor || ',' || CHR(10);
        v_json_registros := v_json_registros || '      "estresse": ' || v_registro_rec.nivel_estresse || ',' || CHR(10);
        v_json_registros := v_json_registros || '      "energia": ' || v_registro_rec.nivel_energia || ',' || CHR(10);
        v_json_registros := v_json_registros || '      "sono_horas": ' || v_registro_rec.horas_sono || ',' || CHR(10);
        v_json_registros := v_json_registros || '      "qualidade_sono": ' || v_registro_rec.qualidade_sono || CHR(10);
        v_json_registros := v_json_registros || '    },' || CHR(10);
        v_json_registros := v_json_registros || '    "observacoes": "' || COALESCE(REPLACE(v_registro_rec.observacoes, '"', '\"'), '') || '"' || CHR(10);
        v_json_registros := v_json_registros || '  }';

        v_primeira_registro := FALSE;
    END LOOP;

    v_json_registros := v_json_registros || CHR(10) || ']';

    -- ========================================================================
    -- SEÇÃO 3: EXPORTAR RECOMENDAÇÕES COM CATEGORIAS
    -- ========================================================================
    v_json_recomendacoes := '[' || CHR(10);

    FOR v_recomendacao_rec IN c_recomendacoes LOOP
        IF NOT v_primeira_recomendacao THEN
            v_json_recomendacoes := v_json_recomendacoes || ',' || CHR(10);
        END IF;

        v_count_recomendacoes := v_count_recomendacoes + 1;

        -- Obter categoria
        BEGIN
            SELECT * INTO v_categoria_recomendacao FROM CATEGORIA_RECOMENDACAO 
            WHERE id_categoria = v_recomendacao_rec.id_categoria;
        EXCEPTION WHEN OTHERS THEN
            v_categoria_recomendacao.id_categoria := NULL;
            v_categoria_recomendacao.nome_categoria := 'Sem categoria';
        END;

        v_json_recomendacoes := v_json_recomendacoes || '  {' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    "_id": "recomendacao_' || v_recomendacao_rec.id_recomendacao || '",' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    "tipo": "recomendacao",' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    "titulo": "' || REPLACE(v_recomendacao_rec.titulo, '"', '\"') || '",' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    "descricao": "' || COALESCE(REPLACE(v_recomendacao_rec.descricao, '"', '\"'), '') || '",' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    "categoria": {' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '      "id": ' || COALESCE(v_categoria_recomendacao.id_categoria, 0) || ',' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '      "nome": "' || REPLACE(COALESCE(v_categoria_recomendacao.nome_categoria, ''), '"', '\"') || '"' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    },' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '    "tipo_conteudo": "' || v_recomendacao_rec.tipo_recomendacao || '"' || CHR(10);
        v_json_recomendacoes := v_json_recomendacoes || '  }';

        v_primeira_recomendacao := FALSE;
    END LOOP;

    v_json_recomendacoes := v_json_recomendacoes || CHR(10) || ']';

    -- ========================================================================
    -- SEÇÃO 4: EXPORTAR PROFISSIONAIS DE SAÚDE
    -- ========================================================================
    v_json_profissionais := '[' || CHR(10);

    FOR v_profissional_rec IN c_profissionais LOOP
        IF NOT v_primeira_profissional THEN
            v_json_profissionais := v_json_profissionais || ',' || CHR(10);
        END IF;

        v_count_profissionais := v_count_profissionais + 1;

        v_json_profissionais := v_json_profissionais || '  {' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "_id": "profissional_' || v_profissional_rec.id_profissional || '",' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "tipo": "profissional_saude",' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "nome": "' || REPLACE(v_profissional_rec.nome, '"', '\"') || '",' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "especialidade": "' || REPLACE(v_profissional_rec.especialidade, '"', '\"') || '",' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "registro": "' || v_profissional_rec.crp_crm || '",' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "contato": {' || CHR(10);
        v_json_profissionais := v_json_profissionais || '      "email": "' || COALESCE(v_profissional_rec.email, '') || '",' || CHR(10);
        v_json_profissionais := v_json_profissionais || '      "telefone": "' || COALESCE(v_profissional_rec.telefone, '') || '"' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    },' || CHR(10);
        v_json_profissionais := v_json_profissionais || '    "disponivel": ' || (CASE WHEN v_profissional_rec.disponivel = 'S' THEN 'true' ELSE 'false' END) || CHR(10);
        v_json_profissionais := v_json_profissionais || '  }';

        v_primeira_profissional := FALSE;
    END LOOP;

    v_json_profissionais := v_json_profissionais || CHR(10) || ']';

    -- ========================================================================
    -- SEÇÃO 5: EXPORTAR ALERTAS
    -- ========================================================================
    v_json_alertas := '[' || CHR(10);

    FOR v_alerta_rec IN c_alertas LOOP
        IF NOT v_primeira_alerta THEN
            v_json_alertas := v_json_alertas || ',' || CHR(10);
        END IF;

        v_count_alertas := v_count_alertas + 1;

        v_json_alertas := v_json_alertas || '  {' || CHR(10);
        v_json_alertas := v_json_alertas || '    "_id": "alerta_' || v_alerta_rec.id_alerta || '",' || CHR(10);
        v_json_alertas := v_json_alertas || '    "id_usuario": ' || v_alerta_rec.id_usuario || ',' || CHR(10);
        v_json_alertas := v_json_alertas || '    "tipo": "' || REPLACE(v_alerta_rec.tipo_alerta, '"', '\"') || '",' || CHR(10);
        v_json_alertas := v_json_alertas || '    "descricao": "' || COALESCE(REPLACE(v_alerta_rec.descricao, '"', '\"'), '') || '",' || CHR(10);
        v_json_alertas := v_json_alertas || '    "nivel_gravidade": "' || v_alerta_rec.nivel_gravidade || '",' || CHR(10);
        v_json_alertas := v_json_alertas || '    "status": "' || v_alerta_rec.status_alerta || '",' || CHR(10);
        v_json_alertas := v_json_alertas || '    "timestamp": "' || TO_CHAR(v_alerta_rec.data_alerta, 'YYYY-MM-DD HH24:MI:SS') || '"' || CHR(10);
        v_json_alertas := v_json_alertas || '  }';

        v_primeira_alerta := FALSE;
    END LOOP;

    v_json_alertas := v_json_alertas || CHR(10) || ']';

    -- ========================================================================
    -- SEÇÃO 6: EXPORTAR SESSÕES DE APOIO
    -- ========================================================================
    v_json_sessoes := '[' || CHR(10);

    FOR v_sessao_rec IN c_sessoes LOOP
        IF NOT v_primeira_sessao THEN
            v_json_sessoes := v_json_sessoes || ',' || CHR(10);
        END IF;

        v_count_sessoes := v_count_sessoes + 1;

        v_json_sessoes := v_json_sessoes || '  {' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "_id": "sessao_' || v_sessao_rec.id_sessao || '",' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "id_usuario": ' || v_sessao_rec.id_usuario || ',' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "id_profissional": ' || v_sessao_rec.id_profissional || ',' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "data_sessao": "' || TO_CHAR(v_sessao_rec.data_sessao, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "duracao_minutos": ' || v_sessao_rec.duracao_minutos || ',' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "tipo_sessao": "' || v_sessao_rec.tipo_sessao || '",' || CHR(10);
        v_json_sessoes := v_json_sessoes || '    "observacoes": "' || COALESCE(REPLACE(v_sessao_rec.observacoes, '"', '\"'), '') || '"' || CHR(10);
        v_json_sessoes := v_json_sessoes || '  }';

        v_primeira_sessao := FALSE;
    END LOOP;

    v_json_sessoes := v_json_sessoes || CHR(10) || ']';

    -- ========================================================================
    -- MONTAR DATASET COMPLETO NO FORMATO MONGODB
    -- ========================================================================
    v_json_completo := '{' || CHR(10);
    v_json_completo := v_json_completo || '  "database": "bemestar_saude_mental",' || CHR(10);
    v_json_completo := v_json_completo || '  "tema": "O Futuro do Trabalho",' || CHR(10);
    v_json_completo := v_json_completo || '  "data_exportacao": "' || TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS') || '",' || CHR(10);
    v_json_completo := v_json_completo || '  "descricao": "Dataset completo para integração com MongoDB",' || CHR(10);

    v_json_completo := v_json_completo || '  "collections": {' || CHR(10);
    v_json_completo := v_json_completo || '    "usuarios": ' || v_json_usuarios || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "registros_bemestar": ' || v_json_registros || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "recomendacoes": ' || v_json_recomendacoes || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "profissionais_saude": ' || v_json_profissionais || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "alertas": ' || v_json_alertas || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "sessoes_apoio": ' || v_json_sessoes || CHR(10);
    v_json_completo := v_json_completo || '  },' || CHR(10);

    v_json_completo := v_json_completo || '  "metadata": {' || CHR(10);
    v_json_completo := v_json_completo || '    "total_usuarios": ' || v_count_usuarios || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "total_registros_bemestar": ' || v_count_registros || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "total_recomendacoes": ' || v_count_recomendacoes || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "total_profissionais": ' || v_count_profissionais || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "total_alertas": ' || v_count_alertas || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "total_sessoes": ' || v_count_sessoes || ',' || CHR(10);
    v_json_completo := v_json_completo || '    "estrutura": "NoSQL - MongoDB",' || CHR(10);
    v_json_completo := v_json_completo || '    "indices_recomendados": [' || CHR(10);
    v_json_completo := v_json_completo || '      "usuarios.id_usuario",' || CHR(10);
    v_json_completo := v_json_completo || '      "registros_bemestar.id_usuario",' || CHR(10);
    v_json_completo := v_json_completo || '      "registros_bemestar.timestamp",' || CHR(10);
    v_json_completo := v_json_completo || '      "recomendacoes.categoria.id",' || CHR(10);
    v_json_completo := v_json_completo || '      "alertas.id_usuario",' || CHR(10);
    v_json_completo := v_json_completo || '      "sessoes_apoio.id_usuario"' || CHR(10);
    v_json_completo := v_json_completo || '    ]' || CHR(10);
    v_json_completo := v_json_completo || '  }' || CHR(10);
    v_json_completo := v_json_completo || '}';

    -- Retornar dataset
    p_dataset := v_json_completo;

    DBMS_OUTPUT.PUT_LINE('✅ Dataset exportado com sucesso para MongoDB!');
    DBMS_OUTPUT.PUT_LINE('Tamanho do dataset: ' || LENGTH(v_json_completo) || ' caracteres');
    DBMS_OUTPUT.PUT_LINE('Total usuários: ' || v_count_usuarios);
    DBMS_OUTPUT.PUT_LINE('Total registros bem-estar: ' || v_count_registros);
    DBMS_OUTPUT.PUT_LINE('Total recomendações: ' || v_count_recomendacoes);
    DBMS_OUTPUT.PUT_LINE('Total profissionais: ' || v_count_profissionais);
    DBMS_OUTPUT.PUT_LINE('Total alertas: ' || v_count_alertas);
    DBMS_OUTPUT.PUT_LINE('Total sessões: ' || v_count_sessoes);

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('❌ Erro ao exportar dataset: ' || SQLERRM);
        p_dataset := '{"erro": "' || REPLACE(SQLERRM, '"', '\"') || '"}';
END SP_EXPORTAR_DATASET_MONGODB;
/

COMMIT;

-- Exportar dataset
SET SERVEROUTPUT ON SIZE 50000;

DECLARE
    v_dataset CLOB;
BEGIN
    SP_EXPORTAR_DATASET_MONGODB(v_dataset);
    -- Exemplo (exibir parte do JSON, pois pode ser grande)
    DBMS_OUTPUT.PUT_LINE(SUBSTR(v_dataset, 1, 5000));
    -- Para salvar no arquivo (requer permissões UTL_FILE):
    -- v_file := UTL_FILE.FOPEN('<DIRETORIO>', 'dataset_mongodb.json', 'W', 32767);
    -- UTL_FILE.PUT(v_file, v_dataset);
    -- UTL_FILE.FCLOSE(v_file);
END;
/

DECLARE
    v_dataset CLOB;
    v_file UTL_FILE.FILE_TYPE;
BEGIN
    SP_EXPORTAR_DATASET_MONGODB(v_dataset);
    v_file := UTL_FILE.FOPEN('/tmp', 'dataset_mongodb.json', 'W', 32767); -- use diretório válido do servidor Oracle
    UTL_FILE.PUT(v_file, v_dataset);
    UTL_FILE.FCLOSE(v_file);
END;
/

SET SERVEROUTPUT ON SIZE 50000;

DECLARE
    v_dataset CLOB;
BEGIN
    SP_EXPORTAR_DATASET_MONGODB(v_dataset);
    -- Exibe o JSON no painel de saída
    DBMS_OUTPUT.PUT_LINE(v_dataset);
END;
/