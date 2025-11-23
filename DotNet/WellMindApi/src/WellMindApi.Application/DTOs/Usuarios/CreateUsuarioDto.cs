namespace WellMindApi.Application.DTOs.Usuarios;

/// <summary>
/// DTO para criação de usuário
/// </summary>
public record CreateUsuarioDto
{
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public string? Genero { get; init; }
    public string? Telefone { get; init; }
}
