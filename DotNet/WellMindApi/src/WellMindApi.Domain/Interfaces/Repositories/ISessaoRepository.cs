using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório de Sessões (Terapia, Coaching, Mentoria)
/// </summary>
public interface ISessaoRepository
{
    /// <summary>
    /// Obtém sessão por ID
    /// </summary>
    Task<Sessao?> ObterPorIdAsync(int id);

    /// <summary>
    /// Obtém todas as sessões de um usuário
    /// </summary>
    Task<IEnumerable<Sessao>> ObterPorUsuarioAsync(int idUsuario);

    /// <summary>
    /// Obtém sessões futuras de um usuário
    /// </summary>
    Task<IEnumerable<Sessao>> ObterFuturasAsync(int idUsuario);

    /// <summary>
    /// Obtém sessões passadas de um usuário
    /// </summary>
    Task<IEnumerable<Sessao>> ObterPassadasAsync(int idUsuario);

    /// <summary>
    /// Obtém sessões por tipo
    /// </summary>
    Task<IEnumerable<Sessao>> ObterPorTipoAsync(int idUsuario, string tipoSessao);

    /// <summary>
    /// Obtém sessões por período
    /// </summary>
    Task<IEnumerable<Sessao>> ObterPorPeriodoAsync(
        int idUsuario,
        DateTime dataInicio,
        DateTime dataFim);

    /// <summary>
    /// Obtém próxima sessão agendada do usuário
    /// </summary>
    Task<Sessao?> ObterProximaSessaoAsync(int idUsuario);

    /// <summary>
    /// Lista todas as sessões com paginação
    /// </summary>
    Task<IEnumerable<Sessao>> ObterTodosAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Adiciona nova sessão
    /// </summary>
    Task AdicionarAsync(Sessao sessao);

    /// <summary>
    /// Atualiza sessão existente
    /// </summary>
    Task AtualizarAsync(Sessao sessao);

    /// <summary>
    /// Remove sessão
    /// </summary>
    Task RemoverAsync(Sessao sessao);

    /// <summary>
    /// Conta total de sessões de um usuário
    /// </summary>
    Task<int> ContarSessoesPorUsuarioAsync(int idUsuario);

    /// <summary>
    /// Obtém estatísticas de sessões por tipo
    /// </summary>
    Task<Dictionary<string, int>> ObterEstatisticasPorTipoAsync(int idUsuario);

    /// <summary>
    /// Verifica se usuário tem sessão agendada em determinada data/hora
    /// </summary>
    Task<bool> TemSessaoAgendadaAsync(int idUsuario, DateTime dataSessao);
}
