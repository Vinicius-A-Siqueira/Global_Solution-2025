using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório de Transições de Carreira
/// </summary>
public interface ITransicaoCarreiraRepository
{
    /// <summary>
    /// Obtém transição por ID
    /// </summary>
    Task<TransicaoCarreira?> ObterPorIdAsync(int id);

    /// <summary>
    /// Obtém todas as transições de um usuário
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterPorUsuarioAsync(int idUsuario);

    /// <summary>
    /// Obtém transições em andamento de um usuário
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterEmAndamentoAsync(int idUsuario);

    /// <summary>
    /// Obtém transições concluídas de um usuário
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterConcluidasAsync(int idUsuario);

    /// <summary>
    /// Obtém transições por tipo
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterPorTipoAsync(
        int idUsuario,
        string tipoTransicao);

    /// <summary>
    /// Obtém transições por período
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterPorPeriodoAsync(
        int idUsuario,
        DateTime dataInicio,
        DateTime dataFim);

    /// <summary>
    /// Obtém transição ativa mais recente do usuário
    /// </summary>
    Task<TransicaoCarreira?> ObterTransicaoAtivaAsync(int idUsuario);

    /// <summary>
    /// Lista todas as transições com paginação
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterTodosAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Adiciona nova transição
    /// </summary>
    Task AdicionarAsync(TransicaoCarreira transicao);

    /// <summary>
    /// Atualiza transição existente
    /// </summary>
    Task AtualizarAsync(TransicaoCarreira transicao);

    /// <summary>
    /// Remove transição
    /// </summary>
    Task RemoverAsync(TransicaoCarreira transicao);

    /// <summary>
    /// Conta total de transições de um usuário
    /// </summary>
    Task<int> ContarTransicoesPorUsuarioAsync(int idUsuario);

    /// <summary>
    /// Obtém estatísticas de transições por tipo
    /// </summary>
    Task<Dictionary<string, int>> ObterEstatisticasPorTipoAsync(int idUsuario);

    /// <summary>
    /// Verifica se usuário tem transição em andamento
    /// </summary>
    Task<bool> TemTransicaoEmAndamentoAsync(int idUsuario);

    /// <summary>
    /// Obtém histórico completo de carreira do usuário
    /// </summary>
    Task<IEnumerable<TransicaoCarreira>> ObterHistoricoCarreiraAsync(int idUsuario);
}
