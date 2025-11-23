namespace WellMindApi.Application.DTOs.Empresas;

/// <summary>
/// DTO para criação de empresa
/// </summary>
public record CreateEmpresaDto
{
    public string NomeEmpresa { get; init; } = string.Empty;
    public string CNPJ { get; init; } = string.Empty;
    public string? Endereco { get; init; }
    public string? Telefone { get; init; }
    public string? EmailContato { get; init; }
}
