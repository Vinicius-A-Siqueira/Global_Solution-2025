using WellMindApi.Application.DTOs.Alertas;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Alertas;

/// <summary>
/// Caso de Uso: Listar alertas com filtros
/// </summary>
public class ListarAlertasUseCase
{
    private readonly IAlertaRepository _repository;

    public ListarAlertasUseCase(IAlertaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AlertaDto>> ExecutarAsync(
        int? idUsuario = null,
        string? status = null,
        bool? apenasAtivos = null)
    {
        IEnumerable<Alerta> alertas;

        // Aplicar filtros
        if (idUsuario.HasValue)
        {
            alertas = await _repository.ObterPorUsuarioAsync(idUsuario.Value);
        }
        else if (!string.IsNullOrEmpty(status))
        {
            alertas = await _repository.ObterPorStatusAsync(status);
        }
        else if (apenasAtivos == true)
        {
            alertas = await _repository.ObterPendentesAsync();
        }
        else
        {
            alertas = await _repository.ObterCriticosAsync();
        }

        return alertas
            .Select(MapearParaDto)
            .OrderByDescending(a => a.DataAlerta)
            .ToList();
    }

    private static AlertaDto MapearParaDto(Alerta alerta)
    {
        return new AlertaDto
        {
            IdAlerta = alerta.IdAlerta,
            IdUsuario = alerta.IdUsuario,
            NomeUsuario = alerta.Usuario?.Nome ?? "",
            TipoAlerta = alerta.TipoAlerta,
            Descricao = alerta.Descricao,
            NivelGravidade = alerta.NivelGravidade,
            Status = alerta.Status,
            DataAlerta = alerta.DataAlerta,
            DataResolucao = alerta.DataResolucao,
            AcaoTomada = alerta.AcaoTomada,
            EhCritico = alerta.EhCritico(),
            EhRecente = alerta.EhRecente(),
            TempoAbertoHoras = (int)alerta.TempoAbertoEmHoras().TotalHours
        };
    }
}
