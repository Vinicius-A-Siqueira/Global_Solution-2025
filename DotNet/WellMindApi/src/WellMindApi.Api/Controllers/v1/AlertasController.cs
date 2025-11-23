using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellMindApi.Application.DTOs.Alertas;
using WellMindApi.Application.UseCases.Alertas;
using WellMindApi.Infrastructure.CrossCutting.Helpers;

namespace WellMindApi.Api.Controllers.v1;

/// <summary>
/// Controller para gerenciamento de alertas
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[Produces("application/json")]
public class AlertasController : ControllerBase
{
    private readonly CriarAlertaUseCase _criarAlertaUseCase;
    private readonly ListarAlertasUseCase _listarAlertasUseCase;
    private readonly ResolverAlertaUseCase _resolverAlertaUseCase;
    private readonly ILogger<AlertasController> _logger;

    public AlertasController(
        CriarAlertaUseCase criarAlertaUseCase,
        ListarAlertasUseCase listarAlertasUseCase,
        ResolverAlertaUseCase resolverAlertaUseCase,
        ILogger<AlertasController> logger)
    {
        _criarAlertaUseCase = criarAlertaUseCase;
        _listarAlertasUseCase = listarAlertasUseCase;
        _resolverAlertaUseCase = resolverAlertaUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo alerta
    /// </summary>
    /// <param name="dto">Dados do alerta</param>
    /// <returns>Alerta criado</returns>
    /// <response code="201">Alerta criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(AlertaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AlertaDto>> Create([FromBody] CreateAlertaDto dto)
    {
        _logger.LogInformation("Criando alerta para usuário: {UserId} - Tipo: {Tipo}, Gravidade: {Gravidade}",
            dto.IdUsuario, dto.TipoAlerta, dto.NivelGravidade);

        try
        {
            var alerta = await _criarAlertaUseCase.ExecutarAsync(dto);

            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            alerta.Links = HateoasHelper.GerarLinksAlerta(alerta.IdAlerta, alerta.Status, baseUrl);

            if (alerta.EhCritico)
            {
                _logger.LogWarning("🚨 ALERTA CRÍTICO criado - ID: {AlertaId}, Usuário: {UserId}",
                    alerta.IdAlerta, alerta.IdUsuario);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = alerta.IdAlerta },
                alerta);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Erro ao criar alerta: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém um alerta por ID
    /// </summary>
    /// <param name="id">ID do alerta</param>
    /// <returns>Dados do alerta</returns>
    /// <response code="200">Alerta encontrado</response>
    /// <response code="404">Alerta não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AlertaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlertaDto>> GetById(int id)
    {
        _logger.LogInformation("Buscando alerta ID: {AlertaId}", id);

        var alertas = await _listarAlertasUseCase.ExecutarAsync();
        var alerta = alertas.FirstOrDefault(a => a.IdAlerta == id);

        if (alerta == null)
        {
            _logger.LogWarning("Alerta ID {AlertaId} não encontrado", id);
            return NotFound(new { message = $"Alerta com ID {id} não encontrado" });
        }

        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        alerta.Links = HateoasHelper.GerarLinksAlerta(alerta.IdAlerta, alerta.Status, baseUrl);

        return Ok(alerta);
    }

    /// <summary>
    /// Lista alertas com filtros opcionais
    /// </summary>
    /// <param name="idUsuario">Filtrar por usuário (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <param name="apenasAtivos">Mostrar apenas alertas ativos (opcional)</param>
    /// <returns>Lista de alertas</returns>
    /// <response code="200">Lista de alertas retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<AlertaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AlertaDto>>> GetAll(
        [FromQuery] int? idUsuario = null,
        [FromQuery] string? status = null,
        [FromQuery] bool? apenasAtivos = null)
    {
        _logger.LogInformation("Listando alertas - Usuário: {UserId}, Status: {Status}, ApenasAtivos: {ApenasAtivos}",
            idUsuario, status, apenasAtivos);

        var alertas = await _listarAlertasUseCase.ExecutarAsync(idUsuario, status, apenasAtivos);

        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        foreach (var alerta in alertas)
        {
            alerta.Links = HateoasHelper.GerarLinksAlerta(alerta.IdAlerta, alerta.Status, baseUrl);
        }

        return Ok(alertas);
    }

    /// <summary>
    /// Atualiza o status de um alerta
    /// </summary>
    /// <param name="id">ID do alerta</param>
    /// <param name="dto">Novos dados de status</param>
    /// <returns>Alerta atualizado</returns>
    /// <response code="200">Alerta atualizado com sucesso</response>
    /// <response code="404">Alerta não encontrado</response>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(AlertaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlertaDto>> UpdateStatus(int id, [FromBody] UpdateAlertaDto dto)
    {
        _logger.LogInformation("Atualizando status do alerta ID: {AlertaId} para {Status}", id, dto.Status);

        try
        {
            var alerta = await _resolverAlertaUseCase.ExecutarAsync(id, dto);

            if (alerta == null)
            {
                _logger.LogWarning("Alerta ID {AlertaId} não encontrado", id);
                return NotFound(new { message = $"Alerta com ID {id} não encontrado" });
            }

            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            alerta.Links = HateoasHelper.GerarLinksAlerta(alerta.IdAlerta, alerta.Status, baseUrl);

            _logger.LogInformation("Alerta ID {AlertaId} atualizado para status: {Status}", id, alerta.Status);
            return Ok(alerta);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Erro ao atualizar alerta: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}
