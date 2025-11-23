namespace WellMindApi.Domain.Entities;

/// <summary>
/// Entidade de Domínio: Alerta
/// Representa um alerta gerado automaticamente ou manualmente sobre o bem-estar do colaborador
/// </summary>
public class Alerta
{
    // Propriedades
    public int IdAlerta { get; private set; }
    public int IdUsuario { get; private set; }
    public string TipoAlerta { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public string NivelGravidade { get; private set; } = "BAIXO";
    public string Status { get; private set; } = "PENDENTE";
    public DateTime DataAlerta { get; private set; }
    public DateTime? DataResolucao { get; private set; }
    public string? AcaoTomada { get; private set; }

    // Relacionamento
    public Usuario? Usuario { get; private set; }

    // Construtor privado para EF Core
    private Alerta() { }

    // ========================================================================
    // FACTORY METHODS
    // ========================================================================

    public static Alerta Criar(
        int idUsuario,
        string tipoAlerta,
        string nivelGravidade,
        string? descricao = null)
    {
        ValidarIdUsuario(idUsuario);
        ValidarTipoAlerta(tipoAlerta);
        ValidarNivelGravidade(nivelGravidade);
        ValidarDescricao(descricao);

        return new Alerta
        {
            IdUsuario = idUsuario,
            TipoAlerta = tipoAlerta.Trim(),
            NivelGravidade = nivelGravidade.ToUpper().Trim(),
            Descricao = descricao?.Trim(),
            Status = "PENDENTE",
            DataAlerta = DateTime.Now
        };
    }

    /// <summary>
    /// Cria alerta de burnout com descrição automática
    /// </summary>
    public static Alerta CriarAlertaBurnout(int idUsuario, RegistroBemEstar registro)
    {
        var descricao = $"Indicadores críticos detectados: " +
                       $"Estresse={registro.NivelEstresse}/10, " +
                       $"Energia={registro.NivelEnergia}/10, " +
                       $"Humor={registro.NivelHumor}/10";

        return Criar(idUsuario, "Risco de Burnout", "CRITICO", descricao);
    }

    // ========================================================================
    // MÉTODOS DE NEGÓCIO
    // ========================================================================

    public void ColocarEmAnalise()
    {
        ValidarTransicaoStatus("EM_ANALISE");
        Status = "EM_ANALISE";
    }

    public void Resolver(string? acaoTomada = null)
    {
        ValidarTransicaoStatus("RESOLVIDO");
        Status = "RESOLVIDO";
        DataResolucao = DateTime.Now;
        AcaoTomada = acaoTomada?.Trim();
    }

    public void Cancelar(string? motivo = null)
    {
        ValidarTransicaoStatus("CANCELADO");
        Status = "CANCELADO";
        DataResolucao = DateTime.Now;
        AcaoTomada = motivo?.Trim();
    }

    public void AtualizarDescricao(string descricao)
    {
        ValidarDescricao(descricao);
        Descricao = descricao.Trim();
    }

    public void EscalarGravidade()
    {
        NivelGravidade = NivelGravidade.ToUpper() switch
        {
            "BAIXO" => "MEDIO",
            "MEDIO" => "ALTO",
            "ALTO" => "CRITICO",
            _ => "CRITICO"
        };
    }

    // ========================================================================
    // PROPRIEDADES CALCULADAS
    // ========================================================================

    public bool EhCritico() => NivelGravidade.ToUpper() == "CRITICO";

    public bool EhRecente() => (DateTime.Now - DataAlerta).TotalHours <= 24;

    public bool EstaPendente() => Status.ToUpper() == "PENDENTE";

    public bool FoiResolvido() => Status.ToUpper() == "RESOLVIDO";

    public TimeSpan TempoAbertoEmHoras()
    {
        var dataFim = DataResolucao ?? DateTime.Now;
        return dataFim - DataAlerta;
    }

    // ========================================================================
    // VALIDAÇÕES PRIVADAS
    // ========================================================================

    private static void ValidarIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new DomainException("ID do usuário inválido");
    }

    private static void ValidarTipoAlerta(string tipo)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new DomainException("Tipo de alerta é obrigatório");

        if (tipo.Length > 50)
            throw new DomainException("Tipo de alerta deve ter no máximo 50 caracteres");
    }

    private static void ValidarNivelGravidade(string nivel)
    {
        if (string.IsNullOrWhiteSpace(nivel))
            throw new DomainException("Nível de gravidade é obrigatório");

        var niveisValidos = new[] { "BAIXO", "MEDIO", "ALTO", "CRITICO" };
        if (!niveisValidos.Contains(nivel.ToUpper()))
            throw new DomainException("Nível de gravidade inválido. Use: BAIXO, MEDIO, ALTO ou CRITICO");
    }

    private static void ValidarDescricao(string? descricao)
    {
        if (descricao?.Length > 500)
            throw new DomainException("Descrição deve ter no máximo 500 caracteres");
    }

    private void ValidarTransicaoStatus(string novoStatus)
    {
        if (Status.ToUpper() == "RESOLVIDO" && novoStatus.ToUpper() != "RESOLVIDO")
            throw new DomainException("Não é possível alterar status de alerta já resolvido");

        if (Status.ToUpper() == "CANCELADO" && novoStatus.ToUpper() != "CANCELADO")
            throw new DomainException("Não é possível alterar status de alerta cancelado");
    }
}
