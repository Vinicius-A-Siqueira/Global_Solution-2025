using AutoMapper;
using WellMindApi.Application.DTOs.Alertas;
using WellMindApi.Application.DTOs.Empresas;
using WellMindApi.Application.DTOs.Registros;
using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.ValueObjects;

namespace WellMindApi.Application.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // ====================================================================
        // USUÁRIOS
        // ====================================================================

        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Idade, opt => opt.MapFrom(src => src.ObterIdade()));

        CreateMap<CreateUsuarioDto, Usuario>()
            .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
            .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
            .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrosBemEstar, opt => opt.Ignore())
            .ForMember(dest => dest.Alertas, opt => opt.Ignore())
            .ForMember(dest => dest.Sessoes, opt => opt.Ignore())
            .ForMember(dest => dest.TransicoesCarreira, opt => opt.Ignore());

        // ====================================================================
        // REGISTROS DE BEM-ESTAR
        // ====================================================================

        CreateMap<RegistroBemEstar, RegistroBemEstarDto>()
            .ForMember(dest => dest.IndiceBemEstar, opt => opt.MapFrom(src => src.CalcularIndiceBemEstar()))
            .ForMember(dest => dest.StatusSaude, opt => opt.MapFrom(src => src.ObterStatusSaude()))
            .ForMember(dest => dest.IndicaBurnout, opt => opt.MapFrom(src => src.IndicaBurnout()))
            .ForMember(dest => dest.AreaCritica, opt => opt.MapFrom(src => src.IdentificarAreaCritica()))
            .ForMember(dest => dest.Recomendacao, opt => opt.MapFrom(src => src.GerarRecomendacao()));

        CreateMap<CreateRegistroBemEstarDto, RegistroBemEstar>()
            .ForMember(dest => dest.IdRegistro, opt => opt.Ignore())
            .ForMember(dest => dest.DataRegistro, opt => opt.Ignore())
            .ForMember(dest => dest.Usuario, opt => opt.Ignore());

        // ====================================================================
        // ALERTAS
        // ====================================================================

        CreateMap<Alerta, AlertaDto>()
            .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nome : ""))
            .ForMember(dest => dest.EhCritico, opt => opt.MapFrom(src => src.EhCritico()))
            .ForMember(dest => dest.EhRecente, opt => opt.MapFrom(src => src.EhRecente()))
            .ForMember(dest => dest.TempoAbertoHoras, opt => opt.MapFrom(src => (int)src.TempoAbertoEmHoras().TotalHours));

        CreateMap<CreateAlertaDto, Alerta>()
            .ForMember(dest => dest.IdAlerta, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.DataAlerta, opt => opt.Ignore())
            .ForMember(dest => dest.DataResolucao, opt => opt.Ignore())
            .ForMember(dest => dest.AcaoTomada, opt => opt.Ignore())
            .ForMember(dest => dest.Usuario, opt => opt.Ignore());

        // ====================================================================
        // EMPRESAS
        // ====================================================================

        CreateMap<Empresa, EmpresaDto>()
            .ForMember(dest => dest.CNPJFormatado, opt => opt.MapFrom(src => FormatarCNPJ(src.CNPJ)));

        CreateMap<CreateEmpresaDto, Empresa>()
            .ForMember(dest => dest.IdEmpresa, opt => opt.Ignore())
            .ForMember(dest => dest.DataCadastro, opt => opt.Ignore());

        // ====================================================================
        // VALUE OBJECTS
        // ====================================================================

        CreateMap<Email, string>().ConvertUsing(src => src.Endereco);
        CreateMap<string, Email>().ConvertUsing(src => Email.Criar(src));

        CreateMap<CNPJ, string>().ConvertUsing(src => src.Numero);
        CreateMap<string, CNPJ>().ConvertUsing(src => CNPJ.Criar(src));
    }

    private static string FormatarCNPJ(string cnpj)
    {
        if (cnpj.Length != 14)
            return cnpj;

        return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
    }
}
