using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório de Recomendações
/// </summary>
public interface IRecomendacaoRepository
{
    /// <summary>
    /// Obtém recomendação por ID
    /// </summary>
    Task<Recomendacao?> ObterPorIdAsync(int id);

    /// <summary>
    /// Obtém todas as recomendações de um usuário
    /// </summary>
    Task<IEnumerable<Recomendacao>> ObterPorUsuarioAsync(int idUsuario);

    /// <summary>
    /// Obtém recomendações não lidas de um usuário
    /// </summary>
    Task<IEnumerable<Recomendacao>> ObterNaoLidasPorUsuarioAsync(int idUsuario);

    /// <summary>
    /// Obtém recomendações por tipo
    /// </summary>
    Task<IEnumerable<Recomendacao>> ObterPorTipoAsync(string tipoRecomendacao);

    /// <summary>
    /// Obtém recomendações recentes (últimos N dias)
    /// </summary>
    Task<IEnumerable<Recomendacao>> ObterRecentesAsync(int idUsuario, int ultimosDias = 7);

    /// <summary>
    /// Lista todas as recomendações com paginação
    /// </summary>
    Task<IEnumerable<Recomendacao>> ObterTodosAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Adiciona nova recomendação
    /// </summary>
    Task AdicionarAsync(Recomendacao recomendacao);

    /// <summary>
    /// Atualiza recomendação existente
    /// </summary>
    Task AtualizarAsync(Recomendacao recomendacao);

    /// <summary>
    /// Remove recomendação
    /// </summary>
    Task RemoverAsync(Recomendacao recomendacao);

    /// <summary>
    /// Marca várias recomendações como lidas
    /// </summary>
    Task MarcarVariasComoLidasAsync(IEnumerable<int> idsRecomendacoes);

    /// <summary>
    /// Conta recomendações não lidas de um usuário
    /// </summary>
    Task<int> ContarNaoLidasAsync(int idUsuario);

    /// <summary>
    /// Obtém estatísticas de recomendações por tipo
    /// </summary>
    Task<Dictionary<string, int>> ObterEstatisticasPorTipoAsync(int idUsuario);
}
