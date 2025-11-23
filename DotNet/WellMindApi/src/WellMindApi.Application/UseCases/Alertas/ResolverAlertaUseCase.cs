using WellMindApi.Application.DTOs.Alertas;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Alertas;

/// <summary>
/// Caso de Uso: Resolver alerta
/// </summary>
public class ResolverAlertaUseCase
{
    private readonly IAlertaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ResolverAlertaUseCase(
        IAlertaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AlertaDto?> ExecutarAsync(int id, UpdateAlertaDto dto)
    {
        var alerta = await _repository.ObterPorIdAsync(id);
        if (alerta == null)
            return null;

        // Aplicar mudança de status (validações de domínio automáticas)
        switch (dto.Status.ToUpper())
        {
            case "EM_ANALISE":
                alerta.ColocarEmAnalise();
                break;

            case "RESOLVIDO":
                alerta.Resolver(dto.AcaoTomada);
                break;

            case "CANCELADO":
                alerta.Cancelar(dto.AcaoTomada);
                break;

            default:
                throw new InvalidOperationException($"Status inválido: {dto.Status}");
        }

        await _repository.AtualizarAsync(alerta);
        await _unitOfWork.CommitAsync();

        return MapearParaDto(alerta);
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
