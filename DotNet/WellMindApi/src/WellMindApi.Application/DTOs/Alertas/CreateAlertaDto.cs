namespace WellMindApi.Application.DTOs.Alertas;

/// <summary>
/// DTO para criação de alerta
/// </summary>
public record CreateAlertaDto
{
    public int IdUsuario { get; init; }
    public string TipoAlerta { get; init; } = string.Empty;
    public string NivelGravidade { get; init; } = string.Empty;
    public string? Descricao { get; init; }
}
