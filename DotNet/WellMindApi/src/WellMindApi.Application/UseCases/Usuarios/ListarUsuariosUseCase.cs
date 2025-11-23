using WellMindApi.Application.DTOs.Common;
using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Usuarios;

/// <summary>
/// Caso de Uso: Listar usuários com paginação
/// </summary>
public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _repository;

    public ListarUsuariosUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<UsuarioDto>> ExecutarAsync(int pageNumber, int pageSize)
    {
        // Validar parâmetros
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        // Obter dados
        var usuarios = await _repository.ObterTodosAsync(pageNumber, pageSize);
        var totalCount = await _repository.ContarAtivosAsync();

        // Mapear para DTOs
        var usuariosDto = usuarios.Select(MapearParaDto).ToList();

        // Retornar resultado paginado
        return PagedResultDto<UsuarioDto>.Create(
            usuariosDto,
            totalCount,
            pageNumber,
            pageSize
        );
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
