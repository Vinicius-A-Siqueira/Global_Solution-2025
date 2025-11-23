namespace WellMindApi.Application.DTOs.Empresas;

/// <summary>
/// DTO de retorno de Empresa
/// </summary>
public record EmpresaDto
{
    public int IdEmpresa { get; init; }
    public string NomeEmpresa { get; init; } = string.Empty;
    public string CNPJ { get; init; } = string.Empty;
    public string CNPJFormatado { get; init; } = string.Empty;
    public string? Endereco { get; init; }
    public string? Telefone { get; init; }
    public string? EmailContato { get; init; }
    public DateTime DataCadastro { get; init; }
}
