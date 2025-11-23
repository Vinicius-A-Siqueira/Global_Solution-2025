using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.Interfaces.Repositories;

public interface IRegistroBemEstarRepository
{
    Task<RegistroBemEstar?> ObterPorIdAsync(int id);
    Task<IEnumerable<RegistroBemEstar>> ObterPorUsuarioAsync(int idUsuario, int ultimosDias = 7);
    Task<IEnumerable<RegistroBemEstar>> ObterTodosAsync(int pageNumber, int pageSize);
    Task<RegistroBemEstar?> ObterUltimoRegistroUsuarioAsync(int idUsuario);
    Task AdicionarAsync(RegistroBemEstar registro);
    Task AtualizarAsync(RegistroBemEstar registro);
    Task RemoverAsync(RegistroBemEstar registro);
    Task<decimal> CalcularMediaIndiceBemEstarAsync(int idUsuario, int ultimosDias = 30);
}
