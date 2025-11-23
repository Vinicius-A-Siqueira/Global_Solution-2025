using WellMindApi.Application.DTOs.Common;

namespace WellMindApi.Application.DTOs.Alertas;

/// <summary>
/// DTO de retorno de Alerta
/// </summary>
public record AlertaDto
{
    public int IdAlerta { get; init; }
    public int IdUsuario { get; init; }
    public string NomeUsuario { get; init; } = string.Empty;
    public string TipoAlerta { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public string NivelGravidade { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime DataAlerta { get; init; }
    public DateTime? DataResolucao { get; init; }
    public string? AcaoTomada { get; init; }

    // Campos calculados
    public bool EhCritico { get; init; }
    public bool EhRecente { get; init; }
    public int TempoAbertoHoras { get; init; }

    // HATEOAS Links
    public Dictionary<string, LinkDto>? Links { get; set; }
}
