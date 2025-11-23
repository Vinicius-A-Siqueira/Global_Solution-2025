using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório de Empresas
/// </summary>
public interface IEmpresaRepository
{
    /// <summary>
    /// Obtém empresa por ID
    /// </summary>
    Task<Empresa?> ObterPorIdAsync(int id);

    /// <summary>
    /// Obtém empresa por CNPJ
    /// </summary>
    Task<Empresa?> ObterPorCNPJAsync(string cnpj);

    /// <summary>
    /// Lista todas as empresas com paginação
    /// </summary>
    Task<IEnumerable<Empresa>> ObterTodosAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Conta total de empresas cadastradas
    /// </summary>
    Task<int> ContarTotalAsync();

    /// <summary>
    /// Adiciona nova empresa
    /// </summary>
    Task AdicionarAsync(Empresa empresa);

    /// <summary>
    /// Atualiza empresa existente
    /// </summary>
    Task AtualizarAsync(Empresa empresa);

    /// <summary>
    /// Remove empresa (soft delete ou hard delete)
    /// </summary>
    Task RemoverAsync(Empresa empresa);

    /// <summary>
    /// Verifica se CNPJ já está cadastrado
    /// </summary>
    Task<bool> CNPJExisteAsync(string cnpj);

    /// <summary>
    /// Busca empresas por nome (like)
    /// </summary>
    Task<IEnumerable<Empresa>> BuscarPorNomeAsync(string nome);

    /// <summary>
    /// Obtém estatísticas de empresas
    /// </summary>
    Task<EmpresaStatistics> ObterEstatisticasAsync(int idEmpresa);
}

/// <summary>
/// Estatísticas de uma empresa
/// </summary>
public class EmpresaStatistics
{
    public int TotalColaboradores { get; set; }
    public int ColaboradoresAtivos { get; set; }
    public int AlertasPendentes { get; set; }
    public int AlertasCriticos { get; set; }
    public decimal IndiceBemEstarMedio { get; set; }
}
