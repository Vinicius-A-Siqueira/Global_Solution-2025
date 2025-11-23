namespace WellMindApi.Application.DTOs.Registros;

/// <summary>
/// DTO para criação de registro de bem-estar
/// </summary>
public record CreateRegistroBemEstarDto
{
    public int IdUsuario { get; init; }
    public int NivelHumor { get; init; }
    public int NivelEstresse { get; init; }
    public int NivelEnergia { get; init; }
    public decimal HorasSono { get; init; }
    public int QualidadeSono { get; init; }
    public string? Observacoes { get; init; }
}
