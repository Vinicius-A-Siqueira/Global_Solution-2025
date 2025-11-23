using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Usuarios;

/// <summary>
/// Caso de Uso: Deletar usuário (soft delete)
/// </summary>
public class DeletarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletarUsuarioUseCase(
        IUsuarioRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecutarAsync(int id)
    {
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario == null)
            return false;

        // Soft delete (desativar usuário)
        usuario.Desativar();

        await _repository.AtualizarAsync(usuario);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
