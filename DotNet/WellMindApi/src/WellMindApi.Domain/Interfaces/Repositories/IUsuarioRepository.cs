using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObterTodosAsync(int pageNumber, int pageSize);
    Task<int> ContarTotalAsync();
    Task<int> ContarAtivosAsync();
    Task AdicionarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task RemoverAsync(Usuario usuario);
    Task<bool> EmailExisteAsync(string email);
    Task<IEnumerable<Usuario>> ObterUsuariosEmRiscoBurnoutAsync();
}
