namespace WellMindApi.Application.DTOs.Usuarios;

/// <summary>
/// DTO para atualização parcial de usuário (PATCH)
/// </summary>
public record PatchUsuarioDto
{
    public string? Nome { get; init; }
    public string? Telefone { get; init; }
    public string? Genero { get; init; }
}
