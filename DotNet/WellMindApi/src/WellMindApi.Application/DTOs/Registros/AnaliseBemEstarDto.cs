namespace WellMindApi.Application.DTOs.Registros;

/// <summary>
/// DTO com análise completa de bem-estar de um usuário
/// </summary>
public record AnaliseBemEstarDto
{
    public int IdUsuario { get; init; }
    public string NomeUsuario { get; init; } = string.Empty;
    public int TotalRegistros { get; init; }
    public decimal IndiceBemEstarMedio { get; init; }
    public string StatusGeralSaude { get; init; } = string.Empty;
    public bool EmRiscoBurnout { get; init; }

    // Médias dos últimos 7 dias
    public decimal MediaHumor { get; init; }
    public decimal MediaEstresse { get; init; }
    public decimal MediaEnergia { get; init; }
    public decimal MediaHorasSono { get; init; }
    public decimal MediaQualidadeSono { get; init; }

    // Tendências
    public string TendenciaHumor { get; init; } = string.Empty; // MELHORANDO, ESTAVEL, PIORANDO
    public string TendenciaEstresse { get; init; } = string.Empty;
    public string TendenciaEnergia { get; init; } = string.Empty;

    // Alertas e recomendações
    public List<string> AreasAtencao { get; init; } = new();
    public List<string> Recomendacoes { get; init; } = new();
}
