namespace WellMindApi.Application.DTOs.Usuarios;

/// <summary>
/// DTO para atualização completa de usuário (PUT)
/// </summary>
public record UpdateUsuarioDto
{
    public string Nome { get; init; } = string.Empty;
    public string? Telefone { get; init; }
    public string? Genero { get; init; }
}
