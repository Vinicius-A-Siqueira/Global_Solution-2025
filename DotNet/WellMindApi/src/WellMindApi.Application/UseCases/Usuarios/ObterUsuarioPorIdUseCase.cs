using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Usuarios;

/// <summary>
/// Caso de Uso: Obter usuário por ID
/// </summary>
public class ObterUsuarioPorIdUseCase
{
    private readonly IUsuarioRepository _repository;

    public ObterUsuarioPorIdUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<UsuarioDto?> ExecutarAsync(int id)
    {
        var usuario = await _repository.ObterPorIdAsync(id);

        if (usuario == null || !usuario.Ativo)
            return null;

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
