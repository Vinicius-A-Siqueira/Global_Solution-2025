namespace WellMindApi.Domain.Entities;

/// <summary>
/// Entidade de Domínio: Registro de Bem-Estar
/// Representa uma avaliação diária do estado emocional e físico do colaborador
/// </summary>
public class RegistroBemEstar
{
    // Propriedades
    public int IdRegistro { get; private set; }
    public int IdUsuario { get; private set; }
    public DateTime DataRegistro { get; private set; }
    public int NivelHumor { get; private set; }
    public int NivelEstresse { get; private set; }
    public int NivelEnergia { get; private set; }
    public decimal HorasSono { get; private set; }
    public int QualidadeSono { get; private set; }
    public string? Observacoes { get; private set; }

    // Relacionamento
    public Usuario? Usuario { get; private set; }

    // Construtor privado para EF Core
    private RegistroBemEstar() { }

    // ========================================================================
    // FACTORY METHOD
    // ========================================================================

    public static RegistroBemEstar Criar(
        int idUsuario,
        int nivelHumor,
        int nivelEstresse,
        int nivelEnergia,
        decimal horasSono,
        int qualidadeSono,
        string? observacoes = null)
    {
        // Validações
        ValidarIdUsuario(idUsuario);
        ValidarNivel(nivelHumor, nameof(nivelHumor));
        ValidarNivel(nivelEstresse, nameof(nivelEstresse));
        ValidarNivel(nivelEnergia, nameof(nivelEnergia));
        ValidarNivel(qualidadeSono, nameof(qualidadeSono));
        ValidarHorasSono(horasSono);
        ValidarObservacoes(observacoes);

        return new RegistroBemEstar
        {
            IdUsuario = idUsuario,
            DataRegistro = DateTime.Now,
            NivelHumor = nivelHumor,
            NivelEstresse = nivelEstresse,
            NivelEnergia = nivelEnergia,
            HorasSono = horasSono,
            QualidadeSono = qualidadeSono,
            Observacoes = observacoes?.Trim()
        };
    }

    // ========================================================================
    // MÉTODOS DE NEGÓCIO (Cálculos e Análises)
    // ========================================================================

    /// <summary>
    /// Calcula índice de bem-estar (0-10) baseado em múltiplos fatores
    /// Fórmula: (Humor + Energia + QualidadeSono - Estresse + IndiceSono) / 4
    /// </summary>
    public decimal CalcularIndiceBemEstar()
    {
        // Normalizar horas de sono para escala 0-10
        var indiceSono = NormalizarHorasSono(HorasSono);

        // Calcular índice
        var indice = (NivelHumor + NivelEnergia + QualidadeSono - NivelEstresse + indiceSono) / 4m;

        // Garantir que está entre 0 e 10
        return Math.Max(0, Math.Min(10, indice));
    }

    /// <summary>
    /// Verifica se os indicadores sugerem risco de burnout
    /// Critérios: Estresse ≥ 8 + Energia ≤ 3 + Humor ≤ 4
    /// </summary>
    public bool IndicaBurnout()
    {
        return NivelEstresse >= 8 && NivelEnergia <= 3 && NivelHumor <= 4;
    }

    /// <summary>
    /// Retorna status de saúde baseado no índice de bem-estar
    /// </summary>
    public string ObterStatusSaude()
    {
        var indice = CalcularIndiceBemEstar();

        return indice switch
        {
            >= 8 => "EXCELENTE",
            >= 6 => "BOM",
            >= 4 => "REGULAR",
            >= 2 => "RUIM",
            _ => "CRÍTICO"
        };
    }

    /// <summary>
    /// Identifica área que precisa de mais atenção
    /// </summary>
    public string IdentificarAreaCritica()
    {
        if (NivelEstresse >= 8)
            return "ESTRESSE";

        if (HorasSono < 5)
            return "SONO";

        if (NivelEnergia <= 3)
            return "ENERGIA";

        if (NivelHumor <= 3)
            return "HUMOR";

        return "NENHUMA";
    }

    /// <summary>
    /// Gera recomendação personalizada baseada nos indicadores
    /// </summary>
    public string GerarRecomendacao()
    {
        var areaCritica = IdentificarAreaCritica();

        return areaCritica switch
        {
            "ESTRESSE" => "Considere praticar técnicas de relaxamento e respiração. Faça pausas regulares durante o trabalho.",
            "SONO" => "Priorize dormir pelo menos 7-8 horas por noite. Evite telas antes de dormir.",
            "ENERGIA" => "Pratique atividades físicas leves e mantenha uma alimentação balanceada.",
            "HUMOR" => "Conecte-se com amigos e familiares. Considere buscar apoio profissional se necessário.",
            _ => "Continue mantendo seus bons hábitos de bem-estar!"
        };
    }

    // ========================================================================
    // MÉTODOS DE ATUALIZAÇÃO
    // ========================================================================

    public void AtualizarObservacoes(string? observacoes)
    {
        ValidarObservacoes(observacoes);
        Observacoes = observacoes?.Trim();
    }

    // ========================================================================
    // VALIDAÇÕES PRIVADAS
    // ========================================================================

    private static void ValidarIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new DomainException("ID do usuário inválido");
    }

    private static void ValidarNivel(int valor, string nomeParametro)
    {
        if (valor < 1 || valor > 10)
            throw new DomainException($"{nomeParametro} deve estar entre 1 e 10");
    }

    private static void ValidarHorasSono(decimal horas)
    {
        if (horas < 0 || horas > 24)
            throw new DomainException("Horas de sono deve estar entre 0 e 24");
    }

    private static void ValidarObservacoes(string? observacoes)
    {
        if (observacoes?.Length > 500)
            throw new DomainException("Observações deve ter no máximo 500 caracteres");
    }

    /// <summary>
    /// Normaliza horas de sono para escala 0-10
    /// Ideal: 7-9 horas = 10 pontos
    /// </summary>
    private static decimal NormalizarHorasSono(decimal horas)
    {
        if (horas >= 7 && horas <= 9)
            return 10;

        if (horas >= 6 && horas < 7)
            return 8;

        if (horas >= 5 && horas < 6)
            return 6;

        if (horas >= 4 && horas < 5)
            return 4;

        if (horas < 4)
            return 2;

        // Mais de 9 horas
        return Math.Max(0, 10 - (horas - 9) * 2);
    }
}
