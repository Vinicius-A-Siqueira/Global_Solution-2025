using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WellMindApi.Application.Mappings;
using WellMindApi.Application.UseCases.Alertas;
using WellMindApi.Application.UseCases.Registros;
using WellMindApi.Application.UseCases.Usuarios;
using WellMindApi.Application.Validators;

namespace WellMindApi.Application;

/// <summary>
/// Extensões para configurar injeção de dependência da camada Application
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(AutoMapperProfile));

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateUsuarioDtoValidator>();

        // Use Cases - Usuarios
        services.AddScoped<CriarUsuarioUseCase>();
        services.AddScoped<ObterUsuarioPorIdUseCase>();
        services.AddScoped<ListarUsuariosUseCase>();
        services.AddScoped<AtualizarUsuarioUseCase>();
        services.AddScoped<DeletarUsuarioUseCase>();
        services.AddScoped<AutenticarUsuarioUseCase>();

        // Use Cases - Registros
        services.AddScoped<CriarRegistroBemEstarUseCase>();
        services.AddScoped<ObterRegistrosUsuarioUseCase>();
        services.AddScoped<AnalisarBemEstarUsuarioUseCase>();

        // Use Cases - Alertas
        services.AddScoped<CriarAlertaUseCase>();
        services.AddScoped<ListarAlertasUseCase>();
        services.AddScoped<ResolverAlertaUseCase>();

        return services;
    }
}
