using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Usuarios;

/// <summary>
/// Caso de Uso: Criar novo usuário
/// </summary>
public class CriarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarUsuarioUseCase(
        IUsuarioRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UsuarioDto> ExecutarAsync(CreateUsuarioDto dto)
    {
        // Validar email único
        if (await _repository.EmailExisteAsync(dto.Email))
        {
            throw new InvalidOperationException($"Email {dto.Email} já está cadastrado");
        }

        // Criar entidade de domínio (validações automáticas)
        var usuario = Usuario.Criar(
            dto.Nome,
            dto.Email,
            dto.Senha,
            dto.DataNascimento,
            dto.Genero,
            dto.Telefone
        );

        // Persistir
        await _repository.AdicionarAsync(usuario);
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
