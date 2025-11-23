using WellMindApi.Application.DTOs.Common;

namespace WellMindApi.Application.DTOs.Usuarios;

public record UsuarioDto
{
    public int IdUsuario { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public int Idade { get; init; }
    public string? Genero { get; init; }
    public string? Telefone { get; init; }
    public DateTime DataCadastro { get; init; }
    public bool Ativo { get; init; }

    // HATEOAS Links
    public Dictionary<string, LinkDto>? Links { get; set; }
}
