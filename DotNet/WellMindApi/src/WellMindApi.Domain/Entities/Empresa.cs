namespace WellMindApi.Domain.Entities;

public class Empresa
{
    public int IdEmpresa { get; private set; }
    public string NomeEmpresa { get; private set; } = string.Empty;
    public string CNPJ { get; private set; } = string.Empty;
    public string? Endereco { get; private set; }
    public string? Telefone { get; private set; }
    public string? EmailContato { get; private set; }
    public DateTime DataCadastro { get; private set; }

    // Construtor privado
    private Empresa() { }

    public static Empresa Criar(
        string nomeEmpresa,
        string cnpj,
        string? endereco = null,
        string? telefone = null,
        string? emailContato = null)
    {
        ValidarNomeEmpresa(nomeEmpresa);
        ValidarCNPJ(cnpj);

        return new Empresa
        {
            NomeEmpresa = nomeEmpresa.Trim(),
            CNPJ = LimparCNPJ(cnpj),
            Endereco = endereco?.Trim(),
            Telefone = telefone?.Trim(),
            EmailContato = emailContato?.ToLower().Trim(),
            DataCadastro = DateTime.Now
        };
    }

    private static void ValidarNomeEmpresa(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome da empresa é obrigatório");

        if (nome.Length > 200)
            throw new DomainException("Nome da empresa deve ter no máximo 200 caracteres");
    }

    private static void ValidarCNPJ(string cnpj)
    {
        var cnpjLimpo = LimparCNPJ(cnpj);

        if (cnpjLimpo.Length != 14)
            throw new DomainException("CNPJ inválido");
    }

    private static string LimparCNPJ(string cnpj)
    {
        return new string(cnpj.Where(char.IsDigit).ToArray());
    }
}
