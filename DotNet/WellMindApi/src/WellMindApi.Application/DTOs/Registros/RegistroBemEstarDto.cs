namespace WellMindApi.Application.DTOs.Registros;

/// <summary>
/// DTO de retorno de Registro de Bem-Estar
/// </summary>
public record RegistroBemEstarDto
{
    public int IdRegistro { get; init; }
    public int IdUsuario { get; init; }
    public DateTime DataRegistro { get; init; }
    public int NivelHumor { get; init; }
    public int NivelEstresse { get; init; }
    public int NivelEnergia { get; init; }
    public decimal HorasSono { get; init; }
    public int QualidadeSono { get; init; }
    public string? Observacoes { get; init; }

    // Campos calculados
    public decimal IndiceBemEstar { get; init; }
    public string StatusSaude { get; init; } = string.Empty;
    public bool IndicaBurnout { get; init; }
    public string AreaCritica { get; init; } = string.Empty;
    public string Recomendacao { get; init; } = string.Empty;
}
