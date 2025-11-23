namespace WellMindApi.Application.DTOs.Alertas;

/// <summary>
/// DTO para atualização de status de alerta
/// </summary>
public record UpdateAlertaDto
{
    public string Status { get; init; } = string.Empty;
    public string? AcaoTomada { get; init; }
}
