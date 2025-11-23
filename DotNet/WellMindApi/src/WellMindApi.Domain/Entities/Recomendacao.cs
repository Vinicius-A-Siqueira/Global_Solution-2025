namespace WellMindApi.Domain.Entities;

public class Recomendacao
{
    public int IdRecomendacao { get; private set; }
    public int IdUsuario { get; private set; }
    public DateTime DataRecomendacao { get; private set; }
    public string TipoRecomendacao { get; private set; } = string.Empty;
    public string Conteudo { get; private set; } = string.Empty;
    public string? Link { get; private set; }
    public bool Lida { get; private set; }

    public Usuario? Usuario { get; private set; }

    private Recomendacao() { }

    public static Recomendacao Criar(
        int idUsuario,
        string tipoRecomendacao,
        string conteudo,
        string? link = null)
    {
        if (idUsuario <= 0)
            throw new DomainException("ID do usuário inválido");

        if (string.IsNullOrWhiteSpace(tipoRecomendacao))
            throw new DomainException("Tipo de recomendação é obrigatório");

        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("Conteúdo é obrigatório");

        return new Recomendacao
        {
            IdUsuario = idUsuario,
            DataRecomendacao = DateTime.Now,
            TipoRecomendacao = tipoRecomendacao.Trim(),
            Conteudo = conteudo.Trim(),
            Link = link?.Trim(),
            Lida = false
        };
    }

    public void MarcarComoLida()
    {
        Lida = true;
    }
}
