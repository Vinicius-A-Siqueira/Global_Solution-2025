namespace WellMindApi.Domain.Entities;

public class TransicaoCarreira
{
    public int IdTransicao { get; private set; }
    public int IdUsuario { get; private set; }
    public DateTime DataTransicao { get; private set; }
    public string TipoTransicao { get; private set; } = string.Empty;
    public string? CargoAnterior { get; private set; }
    public string? CargoNovo { get; private set; }
    public string? Descricao { get; private set; }
    public string StatusTransicao { get; private set; } = string.Empty;

    public Usuario? Usuario { get; private set; }

    private TransicaoCarreira() { }

    public static TransicaoCarreira Criar(
        int idUsuario,
        string tipoTransicao,
        string? cargoAnterior = null,
        string? cargoNovo = null,
        string? descricao = null)
    {
        if (idUsuario <= 0)
            throw new DomainException("ID do usuário inválido");

        if (string.IsNullOrWhiteSpace(tipoTransicao))
            throw new DomainException("Tipo de transição é obrigatório");

        return new TransicaoCarreira
        {
            IdUsuario = idUsuario,
            DataTransicao = DateTime.Now,
            TipoTransicao = tipoTransicao.Trim(),
            CargoAnterior = cargoAnterior?.Trim(),
            CargoNovo = cargoNovo?.Trim(),
            Descricao = descricao?.Trim(),
            StatusTransicao = "EM_ANDAMENTO"
        };
    }

    public void Concluir()
    {
        if (StatusTransicao == "CONCLUIDA")
            throw new DomainException("Transição já foi concluída");

        StatusTransicao = "CONCLUIDA";
    }

    public void Cancelar()
    {
        if (StatusTransicao == "CANCELADA")
            throw new DomainException("Transição já foi cancelada");

        StatusTransicao = "CANCELADA";
    }
}
