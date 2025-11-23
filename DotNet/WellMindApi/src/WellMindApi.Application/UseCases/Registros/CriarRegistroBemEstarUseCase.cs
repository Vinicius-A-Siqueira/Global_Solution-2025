using WellMindApi.Application.DTOs.Registros;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Registros;

/// <summary>
/// Caso de Uso: Criar registro de bem-estar
/// </summary>
public class CriarRegistroBemEstarUseCase
{
    private readonly IRegistroBemEstarRepository _registroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAlertaRepository _alertaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarRegistroBemEstarUseCase(
        IRegistroBemEstarRepository registroRepository,
        IUsuarioRepository usuarioRepository,
        IAlertaRepository alertaRepository,
        IUnitOfWork unitOfWork)
    {
        _registroRepository = registroRepository;
        _usuarioRepository = usuarioRepository;
        _alertaRepository = alertaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegistroBemEstarDto> ExecutarAsync(CreateRegistroBemEstarDto dto)
    {
        // Validar se usuário existe
        var usuario = await _usuarioRepository.ObterPorIdAsync(dto.IdUsuario);
        if (usuario == null || !usuario.Ativo)
        {
            throw new InvalidOperationException($"Usuário {dto.IdUsuario} não encontrado");
        }

        // Criar registro (validações de domínio automáticas)
        var registro = RegistroBemEstar.Criar(
            dto.IdUsuario,
            dto.NivelHumor,
            dto.NivelEstresse,
            dto.NivelEnergia,
            dto.HorasSono,
            dto.QualidadeSono,
            dto.Observacoes
        );

        await _registroRepository.AdicionarAsync(registro);

        // Verificar se precisa criar alerta de burnout
        if (registro.IndicaBurnout())
        {
            var alerta = Alerta.CriarAlertaBurnout(dto.IdUsuario, registro);
            await _alertaRepository.AdicionarAsync(alerta);
        }

        await _unitOfWork.CommitAsync();

        return MapearParaDto(registro);
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
