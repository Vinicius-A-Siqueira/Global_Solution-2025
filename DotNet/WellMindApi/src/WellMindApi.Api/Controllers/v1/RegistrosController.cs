using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellMindApi.Application.DTOs.Registros;
using WellMindApi.Application.UseCases.Registros;

namespace WellMindApi.Api.Controllers.v1;

/// <summary>
/// Controller para gerenciamento de registros de bem-estar
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[Produces("application/json")]
public class RegistrosController : ControllerBase
{
    private readonly CriarRegistroBemEstarUseCase _criarRegistroUseCase;
    private readonly ObterRegistrosUsuarioUseCase _obterRegistrosUseCase;
    private readonly AnalisarBemEstarUsuarioUseCase _analisarBemEstarUseCase;
    private readonly ILogger<RegistrosController> _logger;

    public RegistrosController(
        CriarRegistroBemEstarUseCase criarRegistroUseCase,
        ObterRegistrosUsuarioUseCase obterRegistrosUseCase,
        AnalisarBemEstarUsuarioUseCase analisarBemEstarUseCase,
        ILogger<RegistrosController> logger)
    {
        _criarRegistroUseCase = criarRegistroUseCase;
        _obterRegistrosUseCase = obterRegistrosUseCase;
        _analisarBemEstarUseCase = analisarBemEstarUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo registro de bem-estar
    /// </summary>
    /// <param name="dto">Dados do registro</param>
    /// <returns>Registro criado com análise</returns>
    /// <response code="201">Registro criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(RegistroBemEstarDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegistroBemEstarDto>> Create([FromBody] CreateRegistroBemEstarDto dto)
    {
        _logger.LogInformation("Criando registro de bem-estar para usuário: {UserId}", dto.IdUsuario);

        try
        {
            var registro = await _criarRegistroUseCase.ExecutarAsync(dto);

            _logger.LogInformation("Registro criado - ID: {RegistroId}, Índice: {Indice}, Status: {Status}",
                registro.IdRegistro, registro.IndiceBemEstar, registro.StatusSaude);

            if (registro.IndicaBurnout)
            {
                _logger.LogWarning("⚠️ ALERTA: Registro ID {RegistroId} indica risco de burnout!", registro.IdRegistro);
            }

            return CreatedAtAction(
                nameof(GetByUsuario),
                new { idUsuario = dto.IdUsuario },
                registro);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Erro ao criar registro: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém registros de bem-estar de um usuário
    /// </summary>
    /// <param name="idUsuario">ID do usuário</param>
    /// <param name="ultimosDias">Número de dias para buscar (padrão: 7)</param>
    /// <returns>Lista de registros</returns>
    /// <response code="200">Registros retornados com sucesso</response>
    [HttpGet("usuario/{idUsuario}")]
    [ProducesResponseType(typeof(List<RegistroBemEstarDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RegistroBemEstarDto>>> GetByUsuario(
        int idUsuario,
        [FromQuery] int ultimosDias = 7)
    {
        _logger.LogInformation("Buscando registros do usuário {UserId} - Últimos {Dias} dias", idUsuario, ultimosDias);

        var registros = await _obterRegistrosUseCase.ExecutarAsync(idUsuario, ultimosDias);

        return Ok(registros);
    }

    /// <summary>
    /// Obtém análise completa de bem-estar de um usuário
    /// </summary>
    /// <param name="idUsuario">ID do usuário</param>
    /// <returns>Análise detalhada de bem-estar</returns>
    /// <response code="200">Análise retornada com sucesso</response>
    /// <response code="404">Usuário não encontrado ou sem registros</response>
    [HttpGet("usuario/{idUsuario}/analise")]
    [ProducesResponseType(typeof(AnaliseBemEstarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnaliseBemEstarDto>> GetAnalise(int idUsuario)
    {
        _logger.LogInformation("Gerando análise de bem-estar para usuário: {UserId}", idUsuario);

        var analise = await _analisarBemEstarUseCase.ExecutarAsync(idUsuario);

        if (analise == null)
        {
            _logger.LogWarning("Usuário {UserId} não encontrado ou sem registros", idUsuario);
            return NotFound(new { message = "Usuário não encontrado ou sem registros suficientes" });
        }

        if (analise.EmRiscoBurnout)
        {
            _logger.LogWarning("⚠️ ALERTA CRÍTICO: Usuário {UserId} ({Nome}) em risco de burnout!",
                idUsuario, analise.NomeUsuario);
        }

        return Ok(analise);
    }
}
