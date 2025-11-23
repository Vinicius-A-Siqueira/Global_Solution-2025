using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

public interface IAlertaRepository
{
    Task<Alerta?> ObterPorIdAsync(int id);
    Task<IEnumerable<Alerta>> ObterPorUsuarioAsync(int idUsuario);
    Task<IEnumerable<Alerta>> ObterPendentesAsync();
    Task<IEnumerable<Alerta>> ObterCriticosAsync();
    Task<IEnumerable<Alerta>> ObterPorStatusAsync(string status);
    Task AdicionarAsync(Alerta alerta);
    Task AtualizarAsync(Alerta alerta);
    Task RemoverAsync(Alerta alerta);
    Task<int> ContarAlertasCriticosPendentesAsync();
}
