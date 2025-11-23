using WellMindApi.Application.DTOs.Common;
using WellMindApi.Application.DTOs.Registros;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Registros;

/// <summary>
/// Caso de Uso: Obter registros de bem-estar de um usuário
/// </summary>
public class ObterRegistrosUsuarioUseCase
{
    private readonly IRegistroBemEstarRepository _repository;

    public ObterRegistrosUsuarioUseCase(IRegistroBemEstarRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RegistroBemEstarDto>> ExecutarAsync(int idUsuario, int ultimosDias = 7)
    {
        var registros = await _repository.ObterPorUsuarioAsync(idUsuario, ultimosDias);

        return registros
            .Select(MapearParaDto)
            .OrderByDescending(r => r.DataRegistro)
            .ToList();
    }

    private static RegistroBemEstarDto MapearParaDto(RegistroBemEstar registro)
    {
        return new RegistroBemEstarDto
        {
            IdRegistro = registro.IdRegistro,
            IdUsuario = registro.IdUsuario,
            DataRegistro = registro.DataRegistro,
            NivelHumor = registro.NivelHumor,
            NivelEstresse = registro.NivelEstresse,
            NivelEnergia = registro.NivelEnergia,
            HorasSono = registro.HorasSono,
            QualidadeSono = registro.QualidadeSono,
            Observacoes = registro.Observacoes,
            IndiceBemEstar = registro.CalcularIndiceBemEstar(),
            StatusSaude = registro.ObterStatusSaude(),
            IndicaBurnout = registro.IndicaBurnout(),
            AreaCritica = registro.IdentificarAreaCritica(),
            Recomendacao = registro.GerarRecomendacao()
        };
    }
}
