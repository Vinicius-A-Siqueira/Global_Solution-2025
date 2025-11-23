using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Usuarios;

/// <summary>
/// Caso de Uso: Atualizar usuário
/// </summary>
public class AtualizarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarUsuarioUseCase(
        IUsuarioRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UsuarioDto?> ExecutarAsync(int id, UpdateUsuarioDto dto)
    {
        // Buscar usuário
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario == null)
            return null;

        // Aplicar atualizações (validações de domínio automáticas)
        usuario.AtualizarNome(dto.Nome);

        if (dto.Telefone != null)
            usuario.AtualizarTelefone(dto.Telefone);

        if (dto.Genero != null)
            usuario.AtualizarGenero(dto.Genero);

        // Persistir
        await _repository.AtualizarAsync(usuario);
        await _unitOfWork.CommitAsync();

        // Retornar DTO
        return MapearParaDto(usuario);
    }

    private static UsuarioDto MapearParaDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            IdUsuario = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento,
            Idade = usuario.ObterIdade(),
            Genero = usuario.Genero,
            Telefone = usuario.Telefone,
            DataCadastro = usuario.DataCadastro,
            Ativo = usuario.Ativo
        };
    }
}
