namespace WellMindApi.Domain.Entities;

public class Usuario
{
    public int IdUsuario { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public string? Genero { get; private set; }
    public string? Telefone { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }

    // Relacionamentos (Encapsulados - somente leitura)
    private readonly List<RegistroBemEstar> _registrosBemEstar = new();
    public IReadOnlyCollection<RegistroBemEstar> RegistrosBemEstar => _registrosBemEstar.AsReadOnly();

    private readonly List<Alerta> _alertas = new();
    public IReadOnlyCollection<Alerta> Alertas => _alertas.AsReadOnly();

    private readonly List<Sessao> _sessoes = new();
    public IReadOnlyCollection<Sessao> Sessoes => _sessoes.AsReadOnly();

    private readonly List<TransicaoCarreira> _transicoesCarreira = new();
    public IReadOnlyCollection<TransicaoCarreira> TransicoesCarreira => _transicoesCarreira.AsReadOnly();

    // Construtor privado para EF Core
    private Usuario() { }

    // ========================================================================
    // FACTORY METHODS
    // ========================================================================

    /// <summary>
    /// Cria um novo usuário com validações de negócio
    /// </summary>
    public static Usuario Criar(
        string nome,
        string email,
        string senha,
        DateTime dataNascimento,
        string? genero = null,
        string? telefone = null)
    {
        // Validações de Domínio
        ValidarNome(nome);
        ValidarEmail(email);
        ValidarFormatoSenha(senha);
        ValidarIdade(dataNascimento);

        return new Usuario
        {
            Nome = nome.Trim(),
            Email = email.ToLower().Trim(),
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
            DataNascimento = dataNascimento.Date,
            Genero = genero?.Trim(),
            Telefone = telefone?.Trim(),
            DataCadastro = DateTime.Now,
            Ativo = true
        };
    }

    // ========================================================================
    // MÉTODOS DE NEGÓCIO
    // ========================================================================

    public void AtualizarNome(string nome)
    {
        ValidarNome(nome);
        Nome = nome.Trim();
    }

    public void AtualizarTelefone(string? telefone)
    {
        Telefone = telefone?.Trim();
    }

    public void AtualizarGenero(string? genero)
    {
        Genero = genero?.Trim();
    }

    public bool ValidarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            return false;

        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }

    public void Desativar()
    {
        if (!Ativo)
            throw new DomainException("Usuário já está inativo");

        Ativo = false;
    }

    public void Reativar()
    {
        if (Ativo)
            throw new DomainException("Usuário já está ativo");

        Ativo = true;
    }


    public int ObterIdade()
    {
        return CalcularIdade(DataNascimento);
    }

    // Métodos para adicionar relacionamentos
    public void AdicionarRegistroBemEstar(RegistroBemEstar registro)
    {
        if (registro == null)
            throw new ArgumentNullException(nameof(registro));

        _registrosBemEstar.Add(registro);
    }

    public void AdicionarAlerta(Alerta alerta)
    {
        if (alerta == null)
            throw new ArgumentNullException(nameof(alerta));

        _alertas.Add(alerta);
    }

    public void AdicionarSessao(Sessao sessao)
    {
        if (sessao == null)
            throw new ArgumentNullException(nameof(sessao));

        _sessoes.Add(sessao);
    }

    public void AdicionarTransicaoCarreira(TransicaoCarreira transicao)
    {
        if (transicao == null)
            throw new ArgumentNullException(nameof(transicao));

        _transicoesCarreira.Add(transicao);
    }

    // Análise de Bem-Estar
    public decimal CalcularIndiceBemEstarMedio(int ultimosDias = 7)
    {
        var registrosRecentes = _registrosBemEstar
            .Where(r => r.DataRegistro >= DateTime.Now.AddDays(-ultimosDias))
            .ToList();

        if (!registrosRecentes.Any())
            return 0;

        return registrosRecentes.Average(r => r.CalcularIndiceBemEstar());
    }

    public bool EmRiscoDeBurnout()
    {
        var ultimosRegistros = _registrosBemEstar
            .OrderByDescending(r => r.DataRegistro)
            .Take(3)
            .ToList();

        if (ultimosRegistros.Count < 3)
            return false;

        return ultimosRegistros.All(r => r.IndicaBurnout());
    }

    // ========================================================================
    // VALIDAÇÕES PRIVADAS
    // ========================================================================

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório");

        if (nome.Length < 3)
            throw new DomainException("Nome deve ter no mínimo 3 caracteres");

        if (nome.Length > 100)
            throw new DomainException("Nome deve ter no máximo 100 caracteres");
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email é obrigatório");

        if (!email.Contains("@"))
            throw new DomainException("Email inválido");

        if (email.Length > 100)
            throw new DomainException("Email deve ter no máximo 100 caracteres");
    }

    private static void ValidarFormatoSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new DomainException("Senha é obrigatória");

        if (senha.Length < 6)
            throw new DomainException("Senha deve ter no mínimo 6 caracteres");

        if (senha.Length > 50)
            throw new DomainException("Senha deve ter no máximo 50 caracteres");
    }

    private static void ValidarIdade(DateTime dataNascimento)
    {
        var idade = CalcularIdade(dataNascimento);

        if (idade < 18)
            throw new DomainException("Usuário deve ter no mínimo 18 anos");

        if (idade > 120)
            throw new DomainException("Data de nascimento inválida");
    }

    private static int CalcularIdade(DateTime dataNascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;

        if (dataNascimento.Date > hoje.AddYears(-idade))
            idade--;

        return idade;
    }
}
