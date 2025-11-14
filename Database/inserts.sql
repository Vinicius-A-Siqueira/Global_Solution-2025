SET SERVEROUTPUT ON SIZE 30000;

-- ============================================================================
-- 1. INSERIR 10 USUÁRIOS (Perfis contextualizados)
-- ============================================================================

DECLARE
    v_id_usuario NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE('=== INSERINDO USUÁRIOS ===');

    -- Usuário 1: Desenvolvedor em transformação digital
    PKG_USUARIO.SP_INSERIR(
        'João Silva - Dev Full Stack',
        'joao.silva@techcorp.com.br',
        'hash_pwd_joao_2024',
        TO_DATE('1990-03-15', 'YYYY-MM-DD'),
        'Masculino',
        '11-98765-4321',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U1 João] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 2: Especialista em RH e Inclusão
    PKG_USUARIO.SP_INSERIR(
        'Maria Santos - Gerente RH',
        'maria.santos@techcorp.com.br',
        'hash_pwd_maria_2024',
        TO_DATE('1992-07-22', 'YYYY-MM-DD'),
        'Feminino',
        '11-98765-4322',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U2 Maria] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 3: Profissional em transição de carreira
    PKG_USUARIO.SP_INSERIR(
        'Carlos Oliveira - Career Changer',
        'carlos.oliveira@greentech.com.br',
        'hash_pwd_carlos_2024',
        TO_DATE('1988-11-08', 'YYYY-MM-DD'),
        'Masculino',
        '11-98765-4323',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U3 Carlos] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 4: Jovem profissional em upskilling
    PKG_USUARIO.SP_INSERIR(
        'Ana Costa - Estagiária Dev',
        'ana.costa@startup.com.br',
        'hash_pwd_ana_2024',
        TO_DATE('2003-05-19', 'YYYY-MM-DD'),
        'Feminino',
        '11-98765-4324',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U4 Ana] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 5: Profissional em home office
    PKG_USUARIO.SP_INSERIR(
        'Pedro Ferreira - Consultor',
        'pedro.ferreira@financeflow.com.br',
        'hash_pwd_pedro_2024',
        TO_DATE('1995-09-12', 'YYYY-MM-DD'),
        'Masculino',
        '11-98765-4325',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U5 Pedro] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 6: Profissional com flexibilidade
    PKG_USUARIO.SP_INSERIR(
        'Juliana Rocha - Product Manager',
        'juliana.rocha@cloudsmile.com.br',
        'hash_pwd_juliana_2024',
        TO_DATE('1991-02-28', 'YYYY-MM-DD'),
        'Feminino',
        '11-98765-4326',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U6 Juliana] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 7: Profissional neurodiverso
    PKG_USUARIO.SP_INSERIR(
        'Lucas Gomes - Analista de Dados',
        'lucas.gomes@datadriven.com.br',
        'hash_pwd_lucas_2024',
        TO_DATE('1993-06-14', 'YYYY-MM-DD'),
        'Outro',
        '11-98765-4327',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U7 Lucas] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 8: Profissional em requalificação
    PKG_USUARIO.SP_INSERIR(
        'Fernanda Souza - Trainee Tech',
        'fernanda.souza@futureworks.com.br',
        'hash_pwd_fernanda_2024',
        TO_DATE('1989-12-03', 'YYYY-MM-DD'),
        'Feminino',
        '11-98765-4328',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U8 Fernanda] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 9: Profissional sênior em transformação
    PKG_USUARIO.SP_INSERIR(
        'Gustavo Alves - Tech Lead',
        'gustavo.alves@techcorp.com.br',
        'hash_pwd_gustavo_2024',
        TO_DATE('1985-04-11', 'YYYY-MM-DD'),
        'Masculino',
        '11-98765-4329',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U9 Gustavo] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 10: Profissional com deficiência
    PKG_USUARIO.SP_INSERIR(
        'Camila Ribeiro - Designer Inclusivo',
        'camila.ribeiro@creativestudio.com.br',
        'hash_pwd_camila_2024',
        TO_DATE('1996-08-27', 'YYYY-MM-DD'),
        'Feminino',
        '11-98765-4330',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U10 Camila] ' || v_status || ' - ID: ' || v_id_usuario);

    -- Usuário 11: Profissional adicional para mais dados
    PKG_USUARIO.SP_INSERIR(
        'Rafael Santos - Arquiteto de Software',
        'rafael.santos@techcorp.com.br',
        'hash_pwd_rafael_2024',
        TO_DATE('1987-01-25', 'YYYY-MM-DD'),
        'Masculino',
        '11-98765-4331',
        v_id_usuario, v_status, v_msg
    );
    DBMS_OUTPUT.PUT_LINE('[U11 Rafael] ' || v_status || ' - ID: ' || v_id_usuario);

END;
/

-- ============================================================================
-- 2. INSERIR 10 EMPRESAS (Contextualizadas ao tema)
-- ============================================================================

DECLARE
    v_id_empresa NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO EMPRESAS ===');

    PKG_EMPRESA.SP_INSERIR('TechCorp Inovação Digital', '12.345.678/0001-99',
        'Rua Inovação 100, São Paulo, SP', '11-3333-4444', 'contato@techcorp.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E1] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('FutureWorks Consultoria Organizacional', '98.765.432/0001-11',
        'Av. Futuro 500, Rio de Janeiro, RJ', '21-3333-5555', 'rh@futureworks.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E2] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('Startup Educativa XYZ', '11.111.111/0001-88',
        'Rua Criatividade 50, Belo Horizonte, MG', '31-3333-6666', 'contato@startupxyz.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E3] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('GreenTech Sustentável', '55.555.555/0001-77',
        'Av. Ecologia 200, Curitiba, PR', '41-3333-7777', 'hello@greentech.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E4] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('Human Resources Plus', '44.444.444/0001-66',
        'Rua Pessoas 300, Salvador, BA', '71-3333-8888', 'suporte@hrplus.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E5] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('CloudSmile Tecnologia em Nuvem', '33.333.333/0001-55',
        'Av. Digital 1000, Brasília, DF', '61-3333-9999', 'contato@cloudsmile.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E6] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('FinanceFlow Gestão Financeira', '22.222.222/0001-44',
        'Rua Dinheiro 150, Fortaleza, CE', '85-3333-0000', 'info@financeflow.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E7] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('CreativeStudio Digital', '77.777.777/0001-33',
        'Av. Arte 400, Recife, PE', '81-3333-1111', 'criatividade@creativestudio.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E8] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('DataDriven Solutions', '66.666.666/0001-22',
        'Rua Análise 250, Porto Alegre, RS', '51-3333-2222', 'dados@datadriven.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E9] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('Educação Transformadora 360', '99.999.999/0001-00',
        'Av. Aprendizado 600, Manaus, AM', '92-3333-3333', 'educacao@360transform.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E10] ' || v_status || ' - ID: ' || v_id_empresa);

    PKG_EMPRESA.SP_INSERIR('Inovação Aberta Brasil', '88.888.888/0001-99',
        'Rua Colaboração 777, São Paulo, SP', '11-4444-5555', 'inovacao@aberta.com.br', v_id_empresa, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[E11] ' || v_status || ' - ID: ' || v_id_empresa);

END;
/

-- ============================================================================
-- 3. INSERIR VÍNCULOS USUARIO-EMPRESA (10 registros)
-- ============================================================================

DECLARE
    v_id_vinculo NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO VÍNCULOS USUARIO-EMPRESA ===');

    PKG_USUARIO_EMPRESA.SP_INSERIR(1, 1, 'Desenvolvedor Full Stack', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V1] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(2, 1, 'Gerente de Recursos Humanos', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V2] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(3, 2, 'Consultor Organizacional Sênior', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V3] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(4, 3, 'Instrutora de Programação', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V4] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(5, 4, 'Especialista em Sustentabilidade', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V5] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(6, 6, 'Gerente de Produtos Digital', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V6] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(7, 9, 'Cientista de Dados', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V7] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(8, 2, 'Trainee em Transformação Digital', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V8] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(9, 1, 'Tech Lead', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V9] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(10, 8, 'Designer Inclusivo', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V10] ' || v_status);

    PKG_USUARIO_EMPRESA.SP_INSERIR(11, 1, 'Arquiteto de Software', v_id_vinculo, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[V11] ' || v_status);

END;
/

-- ============================================================================
-- 4. INSERIR CATEGORIAS DE RECOMENDAÇÃO (10)
-- ============================================================================

DECLARE
    v_id_categoria NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO CATEGORIAS DE RECOMENDAÇÃO ===');

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Mindfulness e Meditação',
        'Técnicas de relaxamento e autoconsciência para redução de estresse', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C1] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Exercício Físico',
        'Atividades físicas para aumentar energia e bem-estar emocional', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C2] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Gestão de Tempo',
        'Estratégias para equilibrar vida pessoal e profissional', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C3] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Nutrição e Saúde',
        'Dicas de alimentação saudável para manter foco', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C4] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Desenvolvimento Profissional',
        'Cursos e recursos para upskilling e reskilling', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C5] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Suporte Social',
        'Práticas de conexão com colegas e rede de apoio', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C6] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Sono e Repouso',
        'Orientações para melhor qualidade de sono', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C7] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Resiliência Emocional',
        'Técnicas para lidar com mudanças e incertezas', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C8] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Inclusão e Diversidade',
        'Recursos para criar ambientes inclusivos', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C9] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Comunicação Efetiva',
        'Estratégias para melhorar comunicação em equipes', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C10] ' || v_status);

    PKG_CATEGORIA_RECOMENDACAO.SP_INSERIR('Liderança Ágil',
        'Modelos de liderança para trabalho distribuído', v_id_categoria, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[C11] ' || v_status);

END;
/

-- ============================================================================
-- 5. INSERIR RECOMENDAÇÕES (10)
-- ============================================================================

DECLARE
    v_id_recomendacao NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO RECOMENDAÇÕES ===');

    PKG_RECOMENDACAO.SP_INSERIR(1, 'Meditação Diária de 10 Minutos',
        'Pratique meditação guiada pela manhã para reduzir estresse', 'video',
        'Link: https://meditacao.exemplo.com/diaria', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R1] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(2, 'Caminhada Matinal de 30 Minutos',
        'Caminhe todos os dias para aumentar disposição', 'exercicio',
        'Objetivo: 30 min, ritmo moderado, 5x por semana', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R2] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(3, 'Técnica Pomodoro',
        'Use 25min trabalho + 5min pausa para produtividade', 'texto',
        'Trabalhe 25 minutos, descanse 5 minutos, repita 4 vezes', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R3] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(4, 'Hidratação e Alimentação Balanceada',
        'Beba 2 litros de água e consuma alimentos nutritivos', 'artigo',
        'Coma frutas, vegetais, proteínas e carboidratos integrais', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R4] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(5, 'Curso: Python para Transformação Digital',
        'Aprenda Python para automação e análise de dados', 'artigo',
        'Plataforma: Udemy/Coursera - Duração: 40 horas', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R5] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(6, 'Reunião de Bem-estar em Grupo',
        'Participe semanalmente para conectar com colegas', 'video',
        'Toda terça-feira às 17:00 - Videoconferência', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R6] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(7, 'Higiene do Sono para Profissionais',
        'Mantenha rotina: 8 horas, evite telas 1h antes', 'texto',
        'Deitar e levantar sempre no mesmo horário', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R7] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(8, 'Resiliência em Tempos de Mudança',
        'Workshop sobre como lidar com incertezas', 'video',
        'Duração: 90 minutos - Conteúdo interativo', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R8] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(9, 'Programa de Inclusão e Diversidade',
        'Conheça iniciativas de inclusão da organização', 'artigo',
        'Material sobre direitos, políticas e recursos', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R9] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(10, 'Comunicação Não-Violenta em Equipes',
        'Aprenda técnicas de comunicação efetiva', 'audio',
        'Podcast: 3 episódios de 20 minutos cada', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R10] ' || v_status);

    PKG_RECOMENDACAO.SP_INSERIR(11, 'Liderança Distribuída no Mundo Ágil',
        'Modelos de liderança para trabalho remoto', 'artigo',
        'Guia prático para gestores e equipes', v_id_recomendacao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R11] ' || v_status);

END;
/

-- ============================================================================
-- 6. INSERIR REGISTROS DE BEM-ESTAR (10 por usuário)
-- ============================================================================

DECLARE
    v_id_registro NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
    v_contador NUMBER := 0;
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO REGISTROS DE BEM-ESTAR ===');

    -- Usuário 1 (João) - 3 registros
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(1, 8, 4, 7, 7.5, 8, 'Dia produtivo, sentindo-se energizado', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(1, 7, 5, 6, 6.5, 7, 'Trabalho pesado, um pouco cansado', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(1, 9, 2, 8, 8.0, 9, 'Excelente dia! Bem descansado', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 2 (Maria) - 3 registros
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(2, 7, 6, 7, 7.0, 8, 'Dia normal de trabalho', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(2, 8, 3, 8, 8.5, 9, 'Muito feliz com novos projetos de inclusão', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(2, 8, 4, 8, 8.0, 8, 'Colaboração produtiva com time', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 3 (Carlos) - em transição
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(3, 6, 7, 5, 5.5, 6, 'Estresse com transição de carreira', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(3, 7, 5, 7, 7.0, 8, 'Melhorando com acompanhamento profissional', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(3, 8, 4, 7, 7.5, 8, 'Adaptando bem à nova área', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 4 (Ana) - jovem entusiasmada
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(4, 9, 2, 9, 8.0, 9, 'Jovem profissional muito entusiasmada', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    PKG_REGISTRO_BEMESTAR.SP_INSERIR(4, 8, 3, 8, 7.5, 8, 'Aprendendo novas tecnologias', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 5 (Pedro)
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(5, 7, 4, 7, 7.5, 8, 'Rotina equilibrada em home office', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 6 (Juliana)
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(6, 8, 3, 8, 8.0, 8, 'Dia produtivo com flexibilidade', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 7 (Lucas)
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(7, 7, 5, 6, 6.5, 7, 'Foco em análise de dados', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

    -- Usuário 8 (Fernanda)
    PKG_REGISTRO_BEMESTAR.SP_INSERIR(8, 8, 4, 7, 7.0, 8, 'Progredindo em trainee', v_id_registro, v_status, v_msg);
    v_contador := v_contador + 1;
    DBMS_OUTPUT.PUT_LINE('[Reg ' || v_contador || '] ' || v_status);

END;
/

-- ============================================================================
-- 7. INSERIR PROFISSIONAIS DE SAÚDE (10)
-- ============================================================================

DECLARE
    v_id_profissional NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO PROFISSIONAIS DE SAÚDE ===');

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dra. Fernanda Silva', 'Psicologia Organizacional',
        'CRP/05-123456', 'fernanda.silva@saude.com.br', '11-98765-4321', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P1] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dr. Carlos Mendes', 'Psiquiatria',
        'CRM/SP-123456', 'carlos.mendes@saude.com.br', '11-98765-4322', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P2] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dra. Amanda Costa', 'Terapia Cognitivo-Comportamental',
        'CRP/05-654321', 'amanda.costa@saude.com.br', '11-98765-4323', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P3] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dr. Ricardo Gomes', 'Medicina do Trabalho',
        'CRM/SP-654321', 'ricardo.gomes@saude.com.br', '11-98765-4324', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P4] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dra. Juliana Alves', 'Coaching Executivo',
        'CRP/05-987654', 'juliana.alves@saude.com.br', '11-98765-4325', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P5] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dr. Roberto Santos', 'Nutrição (Coach)',
        'CRN-3-987654', 'roberto.santos@saude.com.br', '11-98765-4326', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P6] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dra. Patricia Oliveira', 'Terapia de Grupo',
        'CRP/05-321987', 'patricia.oliveira@saude.com.br', '11-98765-4327', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P7] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dr. Felipe Martins', 'Mindfulness e Meditação',
        'CRP/05-456123', 'felipe.martins@saude.com.br', '11-98765-4328', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P8] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dra. Mariana Ferreira', 'Psicologia Positiva',
        'CRP/05-789456', 'mariana.ferreira@saude.com.br', '11-98765-4329', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P9] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dr. Lucas Rocha', 'Gestão de Crises',
        'CRP/05-654987', 'lucas.rocha@saude.com.br', '11-98765-4330', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P10] ' || v_status);

    PKG_PROFISSIONAL_SAUDE.SP_INSERIR('Dra. Sabrina Alves', 'Psicodrama Corporativo',
        'CRP/05-111222', 'sabrina.alves@saude.com.br', '11-98765-4331', 'S', v_id_profissional, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[P11] ' || v_status);

END;
/

-- ============================================================================
-- 8. INSERIR CONQUISTAS (10)
-- ============================================================================

DECLARE
    v_id_conquista NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO CONQUISTAS (GAMIFICAÇÃO) ===');

    PKG_CONQUISTA.SP_INSERIR('Primeiro Registro', 'Parabéns! Você fez seu primeiro registro de bem-estar', '🎯', 10, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq1] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('7 Dias Consecutivos', 'Registrou bem-estar por 7 dias seguidos - disciplina!', '📅', 50, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq2] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('30 Dias Dedicado', 'Um mês completo - você é um campeão!', '🏆', 100, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq3] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Humor Sempre Positivo', 'Manteve humor acima de 7 por 14 dias - ótimo ânimo!', '😊', 75, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq4] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Estresse Controlado', 'Reduziu estresse para níveis baixos - excelente!', '🧘', 75, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq5] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Noites Bem Dormidas', 'Dormiu bem (>6h) por 10 noites - recuperação!', '😴', 60, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq6] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Energia em Alta', 'Manteve energia acima de 8 por 7 dias - disposição!', '⚡', 70, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq7] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Aprendiz de Bem-estar', 'Completou 5 recomendações - aprendendo!', '📚', 55, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq8] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Sessentão de Sessões', 'Completou 5 sessões de apoio - crescimento!', '🤝', 80, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq9] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Mestre do Equilíbrio', 'Alcançou índice de bem-estar > 8 - parabéns!', '👑', 150, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq10] ' || v_status);

    PKG_CONQUISTA.SP_INSERIR('Transformação Digital', 'Completou curso de upskilling - futuro!', '💻', 100, v_id_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[Conq11] ' || v_status);

END;
/

-- ============================================================================
-- 9. INSERIR RECOMENDAÇÃO_USUARIO (10 registros)
-- ============================================================================

DECLARE
    v_id_recomendacao_usuario NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE('=== INSERINDO RECOMENDAÇÕES PARA USUÁRIOS ===');

    -- Usuário 1 (João) recebe recomendações
    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(1, 1, 'N', NULL, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U1] ' || v_status);

    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(1, 2, 'N', NULL, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U2] ' || v_status);

    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(1, 3, 'S', 5, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U3] ' || v_status || ' (Lida e avaliada)');

    -- Usuário 2 (Maria) recebe recomendações
    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(2, 4, 'S', 4, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U4] ' || v_status);

    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(2, 5, 'N', NULL, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U5] ' || v_status);

    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(2, 6, 'S', 5, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U6] ' || v_status);

    -- Usuário 3 (Carlos) - em transição de carreira
    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(3, 8, 'S', 4, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U7] ' || v_status || ' (Resiliência)');

    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(3, 5, 'S', 5, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U8] ' || v_status || ' (Upskilling)');

    -- Usuário 4 (Ana) - jovem
    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(4, 5, 'S', 5, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U9] ' || v_status || ' (Jovem aprendendo)');

    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(4, 11, 'N', NULL, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U10] ' || v_status);

    -- Usuário 5 (Pedro)
    PKG_RECOMENDACAO_USUARIO.SP_INSERIR(5, 7, 'S', 5, v_id_recomendacao_usuario, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[R_U11] ' || v_status);

END;
/

-- ============================================================================
-- 10. INSERIR ALERTAS (10 registros)
-- ============================================================================

DECLARE
    v_id_alerta NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO ALERTAS ===');

    -- Alerta para Usuário 3 (Carlos) - em transição
    PKG_ALERTA.SP_INSERIR(3, 'Estresse Elevado', 
        'Nível de estresse detectado em 7/10. Recomenda-se atividades de relaxamento.',
        'ALTO', 'EM_ANALISE', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A1] ' || v_status || ' - Estresse');

    PKG_ALERTA.SP_INSERIR(3, 'Transição de Carreira', 
        'Usuário em fase de mudança profissional. Ofereça suporte especial.',
        'MEDIO', 'PENDENTE', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A2] ' || v_status || ' - Transição');

    -- Alertas para diferentes usuários
    PKG_ALERTA.SP_INSERIR(1, 'Padrão de Sono', 
        'Registrado sono abaixo do esperado nos últimos dias.',
        'BAJO', 'PENDENTE', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A3] ' || v_status);

    PKG_ALERTA.SP_INSERIR(2, 'Bem-estar Positivo', 
        'Usuário mantendo indicadores de bem-estar em bom nível. Parabéns!',
        'BAJO', 'RESOLVIDO', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A4] ' || v_status);

    PKG_ALERTA.SP_INSERIR(4, 'Muito Motivado', 
        'Jovem profissional com excelentes indicadores de energia e humor.',
        'BAJO', 'RESOLVIDO', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A5] ' || v_status);

    PKG_ALERTA.SP_INSERIR(5, 'Home Office', 
        'Profissional em home office - verificar isolamento social.',
        'MEDIO', 'PENDENTE', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A6] ' || v_status);

    PKG_ALERTA.SP_INSERIR(6, 'Flexibilidade Positiva', 
        'Product Manager adaptando bem ao novo modelo flexible.',
        'BAJO', 'RESOLVIDO', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A7] ' || v_status);

    PKG_ALERTA.SP_INSERIR(7, 'Análise de Dados', 
        'Profissional neurodiverso apresentando ótimo desempenho.',
        'BAJO', 'RESOLVIDO', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A8] ' || v_status);

    PKG_ALERTA.SP_INSERIR(8, 'Trainee em Desenvolvimento', 
        'Trainee em fase de transição para área tech - apoio contínuo.',
        'MEDIO', 'EM_ANALISE', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A9] ' || v_status);

    PKG_ALERTA.SP_INSERIR(9, 'Tech Lead Sênior', 
        'Tech Lead monitorando bem-estar da equipe - modelo positivo.',
        'BAJO', 'RESOLVIDO', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A10] ' || v_status);

    PKG_ALERTA.SP_INSERIR(10, 'Inclusão Bem-sucedida', 
        'Designer com deficiência apresenta excelente adaptação.',
        'BAJO', 'RESOLVIDO', v_id_alerta, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[A11] ' || v_status);

END;
/

-- ============================================================================
-- 11. INSERIR CONQUISTAS DOS USUÁRIOS (10 registros)
-- ============================================================================

DECLARE
    v_id_usuario_conquista NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO CONQUISTAS DOS USUÁRIOS ===');

    -- João conquistou
    PKG_USUARIO_CONQUISTA.SP_INSERIR(1, 1, 10, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC1] ' || v_status || ' (João - Primeiro Registro)');

    PKG_USUARIO_CONQUISTA.SP_INSERIR(1, 2, 50, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC2] ' || v_status || ' (João - 7 Dias)');

    PKG_USUARIO_CONQUISTA.SP_INSERIR(1, 4, 75, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC3] ' || v_status || ' (João - Humor Positivo)');

    -- Maria conquistou
    PKG_USUARIO_CONQUISTA.SP_INSERIR(2, 1, 10, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC4] ' || v_status || ' (Maria - Primeiro Registro)');

    PKG_USUARIO_CONQUISTA.SP_INSERIR(2, 2, 50, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC5] ' || v_status || ' (Maria - 7 Dias)');

    PKG_USUARIO_CONQUISTA.SP_INSERIR(2, 5, 75, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC6] ' || v_status || ' (Maria - Estresse Controlado)');

    -- Carlos conquistou (mesmo em transição)
    PKG_USUARIO_CONQUISTA.SP_INSERIR(3, 1, 10, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC7] ' || v_status || ' (Carlos - Primeiro Registro)');

    PKG_USUARIO_CONQUISTA.SP_INSERIR(3, 8, 55, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC8] ' || v_status || ' (Carlos - Aprendiz de Bem-estar)');

    -- Ana conquistou
    PKG_USUARIO_CONQUISTA.SP_INSERIR(4, 1, 10, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC9] ' || v_status || ' (Ana - Primeiro Registro)');

    PKG_USUARIO_CONQUISTA.SP_INSERIR(4, 11, 100, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC10] ' || v_status || ' (Ana - Transformação Digital)');

    -- Pedro conquistou
    PKG_USUARIO_CONQUISTA.SP_INSERIR(5, 6, 60, v_id_usuario_conquista, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[UC11] ' || v_status || ' (Pedro - Noites Bem Dormidas)');

END;
/

-- ============================================================================
-- 12. INSERIR SESSÕES DE APOIO (10 registros)
-- ============================================================================

DECLARE
    v_id_sessao NUMBER;
    v_status VARCHAR2(100);
    v_msg VARCHAR2(4000);
BEGIN
    DBMS_OUTPUT.PUT_LINE(CHR(10) || '=== INSERINDO SESSÕES DE APOIO ===');

    -- Carlos (em transição) com Psicóloga Organizacional
    PKG_SESSAO_APOIO.SP_INSERIR(3, 1, TO_TIMESTAMP('2025-11-14 14:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        60, 'individual', 'Discussão sobre transição de carreira', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S1] ' || v_status || ' - Carlos com Psicóloga');

    -- Carlos com Psiquiatra para avaliação
    PKG_SESSAO_APOIO.SP_INSERIR(3, 2, TO_TIMESTAMP('2025-11-15 10:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        45, 'individual', 'Avaliação de bem-estar durante transição', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S2] ' || v_status || ' - Carlos com Psiquiatra');

    -- João com Coach Executivo
    PKG_SESSAO_APOIO.SP_INSERIR(1, 5, TO_TIMESTAMP('2025-11-14 15:30:00', 'YYYY-MM-DD HH24:MI:SS'),
        60, 'individual', 'Coaching de liderança técnica', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S3] ' || v_status || ' - João com Coach');

    -- Maria com Psicóloga Organizacional
    PKG_SESSAO_APOIO.SP_INSERIR(2, 1, TO_TIMESTAMP('2025-11-14 11:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        45, 'individual', 'Gestão de equipes em bem-estar', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S4] ' || v_status || ' - Maria com Psicóloga');

    -- Grupo de bem-estar com vários participantes
    PKG_SESSAO_APOIO.SP_INSERIR(1, 7, TO_TIMESTAMP('2025-11-16 17:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        90, 'grupo', 'Terapia de grupo sobre bem-estar no trabalho', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S5] ' || v_status || ' - Grupo com Patricia');

    -- Pedro com Nutricionista/Coach
    PKG_SESSAO_APOIO.SP_INSERIR(5, 6, TO_TIMESTAMP('2025-11-17 09:30:00', 'YYYY-MM-DD HH24:MI:SS'),
        30, 'individual', 'Orientação nutricional para profissional em home office', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S6] ' || v_status || ' - Pedro com Nutricionista');

    -- Ana com Mindfulness
    PKG_SESSAO_APOIO.SP_INSERIR(4, 8, TO_TIMESTAMP('2025-11-14 18:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        60, 'individual', 'Meditação guiada para jovem profissional', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S7] ' || v_status || ' - Ana com Felipe (Mindfulness)');

    -- Gustavo com Psicologia Positiva
    PKG_SESSAO_APOIO.SP_INSERIR(9, 9, TO_TIMESTAMP('2025-11-15 14:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        60, 'individual', 'Sessão de psicologia positiva para liderança', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S8] ' || v_status || ' - Gustavo com Mariana');

    -- Camila (com deficiência) com especialista em inclusão
    PKG_SESSAO_APOIO.SP_INSERIR(10, 1, TO_TIMESTAMP('2025-11-16 10:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        45, 'individual', 'Apoio para inclusão e adaptação no trabalho', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S9] ' || v_status || ' - Camila com Psicóloga');

    -- Rafael com Gestão de Crises
    PKG_SESSAO_APOIO.SP_INSERIR(11, 10, TO_TIMESTAMP('2025-11-15 16:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        60, 'acompanhamento', 'Acompanhamento de arquiteto de software', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S10] ' || v_status || ' - Rafael com Lucas');

    -- Sessão de emergência para alguém em crise
    PKG_SESSAO_APOIO.SP_INSERIR(7, 3, TO_TIMESTAMP('2025-11-14 19:00:00', 'YYYY-MM-DD HH24:MI:SS'),
        45, 'emergencia', 'Sessão de emergência - suporte em crise', v_id_sessao, v_status, v_msg);
    DBMS_OUTPUT.PUT_LINE('[S11] ' || v_status || ' - Emergência para Lucas');

END;
/

COMMIT;

