using WellMindApi.Application.DTOs.Alertas;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Alertas;

/// <summary>
/// Caso de Uso: Criar alerta
/// </summary>
public class CriarAlertaUseCase
{
    private readonly IAlertaRepository _alertaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarAlertaUseCase(
        IAlertaRepository alertaRepository,
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
    {
        _alertaRepository = alertaRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AlertaDto> ExecutarAsync(CreateAlertaDto dto)
    {
        // Validar se usuário existe
        var usuario = await _usuarioRepository.ObterPorIdAsync(dto.IdUsuario);
        if (usuario == null || !usuario.Ativo)
        {
            throw new InvalidOperationException($"Usuário {dto.IdUsuario} não encontrado");
        }

        // Criar alerta (validações de domínio automáticas)
        var alerta = Alerta.Criar(
            dto.IdUsuario,
            dto.TipoAlerta,
            dto.NivelGravidade,
            dto.Descricao
        );

        await _alertaRepository.AdicionarAsync(alerta);
        await _unitOfWork.CommitAsync();

        return MapearParaDto(alerta, usuario.Nome);
    }

    private static AlertaDto MapearParaDto(Alerta alerta, string nomeUsuario)
    {
        return new AlertaDto
        {
            IdAlerta = alerta.IdAlerta,
            IdUsuario = alerta.IdUsuario,
            NomeUsuario = nomeUsuario,
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
