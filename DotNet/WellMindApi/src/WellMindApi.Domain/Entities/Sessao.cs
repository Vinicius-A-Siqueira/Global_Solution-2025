namespace WellMindApi.Domain.Entities;

public class Sessao
{
    public int IdSessao { get; private set; }
    public int IdUsuario { get; private set; }
    public DateTime DataSessao { get; private set; }
    public string TipoSessao { get; private set; } = string.Empty;
    public string? Tema { get; private set; }
    public string? Observacoes { get; private set; }
    public string? ProximosPassos { get; private set; }

    public Usuario? Usuario { get; private set; }

    private Sessao() { }

    public static Sessao Criar(
        int idUsuario,
        DateTime dataSessao,
        string tipoSessao,
        string? tema = null)
    {
        if (idUsuario <= 0)
            throw new DomainException("ID do usuário inválido");

        if (string.IsNullOrWhiteSpace(tipoSessao))
            throw new DomainException("Tipo de sessão é obrigatório");

        return new Sessao
        {
            IdUsuario = idUsuario,
            DataSessao = dataSessao,
            TipoSessao = tipoSessao.Trim(),
            Tema = tema?.Trim()
        };
    }

    public void RegistrarObservacoes(string observacoes)
    {
        if (observacoes?.Length > 1000)
            throw new DomainException("Observações devem ter no máximo 1000 caracteres");

        Observacoes = observacoes?.Trim();
    }

    public void DefinirProximosPassos(string proximosPassos)
    {
        if (proximosPassos?.Length > 500)
            throw new DomainException("Próximos passos devem ter no máximo 500 caracteres");

        ProximosPassos = proximosPassos?.Trim();
    }
}
