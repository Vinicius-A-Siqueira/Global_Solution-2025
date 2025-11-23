using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellMindApi.Application.DTOs.Common;
using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Application.UseCases.Usuarios;
using WellMindApi.Infrastructure.CrossCutting.Helpers;

namespace WellMindApi.Api.Controllers.v1;

/// <summary>
/// Controller para gerenciamento de usuários
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
    private readonly CriarUsuarioUseCase _criarUsuarioUseCase;
    private readonly ObterUsuarioPorIdUseCase _obterUsuarioPorIdUseCase;
    private readonly ListarUsuariosUseCase _listarUsuariosUseCase;
    private readonly AtualizarUsuarioUseCase _atualizarUsuarioUseCase;
    private readonly DeletarUsuarioUseCase _deletarUsuarioUseCase;
    private readonly AutenticarUsuarioUseCase _autenticarUsuarioUseCase;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(
        CriarUsuarioUseCase criarUsuarioUseCase,
        ObterUsuarioPorIdUseCase obterUsuarioPorIdUseCase,
        ListarUsuariosUseCase listarUsuariosUseCase,
        AtualizarUsuarioUseCase atualizarUsuarioUseCase,
        DeletarUsuarioUseCase deletarUsuarioUseCase,
        AutenticarUsuarioUseCase autenticarUsuarioUseCase,
        ILogger<UsuariosController> logger)
    {
        _criarUsuarioUseCase = criarUsuarioUseCase;
        _obterUsuarioPorIdUseCase = obterUsuarioPorIdUseCase;
        _listarUsuariosUseCase = listarUsuariosUseCase;
        _atualizarUsuarioUseCase = atualizarUsuarioUseCase;
        _deletarUsuarioUseCase = deletarUsuarioUseCase;
        _autenticarUsuarioUseCase = autenticarUsuarioUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Autentica um usuário e retorna token JWT
    /// </summary>
    /// <param name="dto">Credenciais de login</param>
    /// <returns>Token JWT e dados do usuário</returns>
    /// <response code="200">Autenticação realizada com sucesso</response>
    /// <response code="401">Credenciais inválidas</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
    {
        _logger.LogInformation("Tentativa de login para email: {Email}", dto.Email);

        var response = await _autenticarUsuarioUseCase.ExecutarAsync(dto);

        if (response == null)
        {
            _logger.LogWarning("Login falhou para email: {Email}", dto.Email);
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }

        _logger.LogInformation("Login bem-sucedido para usuário ID: {UserId}", response.Usuario.IdUsuario);
        return Ok(response);
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="dto">Dados do usuário</param>
    /// <returns>Usuário criado</returns>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="409">Email já cadastrado</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UsuarioDto>> Create([FromBody] CreateUsuarioDto dto)
    {
        _logger.LogInformation("Criando novo usuário: {Email}", dto.Email);

        try
        {
            var usuario = await _criarUsuarioUseCase.ExecutarAsync(dto);

            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            usuario.Links = HateoasHelper.GerarLinksUsuario(usuario.IdUsuario, baseUrl);

            _logger.LogInformation("Usuário criado com sucesso: ID {UserId}", usuario.IdUsuario);

            return CreatedAtAction(
                nameof(GetById),
                new { id = usuario.IdUsuario },
                usuario);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao criar usuário: {Message}", ex.Message);
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém um usuário por ID
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Dados do usuário</returns>
    /// <response code="200">Usuário encontrado</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        _logger.LogInformation("Buscando usuário ID: {UserId}", id);

        var usuario = await _obterUsuarioPorIdUseCase.ExecutarAsync(id);

        if (usuario == null)
        {
            _logger.LogWarning("Usuário ID {UserId} não encontrado", id);
            return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
        }

        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        usuario.Links = HateoasHelper.GerarLinksUsuario(usuario.IdUsuario, baseUrl);

        return Ok(usuario);
    }

    /// <summary>
    /// Lista todos os usuários com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
    /// <returns>Lista paginada de usuários</returns>
    /// <response code="200">Lista de usuários retornada com sucesso</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedResultDto<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<UsuarioDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Listando usuários - Página: {PageNumber}, Tamanho: {PageSize}", pageNumber, pageSize);

        var result = await _listarUsuariosUseCase.ExecutarAsync(pageNumber, pageSize);

        // Adicionar links HATEOAS em cada usuário
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        foreach (var usuario in result.Items)
        {
            usuario.Links = HateoasHelper.GerarLinksUsuario(usuario.IdUsuario, baseUrl);
        }

        return Ok(result);
    }

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Usuário atualizado</returns>
    /// <response code="200">Usuário atualizado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> Update(int id, [FromBody] UpdateUsuarioDto dto)
    {
        _logger.LogInformation("Atualizando usuário ID: {UserId}", id);

        var usuario = await _atualizarUsuarioUseCase.ExecutarAsync(id, dto);

        if (usuario == null)
        {
            _logger.LogWarning("Usuário ID {UserId} não encontrado para atualização", id);
            return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
        }

        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        usuario.Links = HateoasHelper.GerarLinksUsuario(usuario.IdUsuario, baseUrl);

        _logger.LogInformation("Usuário ID {UserId} atualizado com sucesso", id);
        return Ok(usuario);
    }

    /// <summary>
    /// Deleta um usuário (soft delete)
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Status da operação</returns>
    /// <response code="204">Usuário deletado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deletando usuário ID: {UserId}", id);

        var sucesso = await _deletarUsuarioUseCase.ExecutarAsync(id);

        if (!sucesso)
        {
            _logger.LogWarning("Usuário ID {UserId} não encontrado para deleção", id);
            return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
        }

        _logger.LogInformation("Usuário ID {UserId} deletado com sucesso", id);
        return NoContent();
    }
}
