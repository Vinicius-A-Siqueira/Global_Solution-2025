using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WellMindApi.Application.Interfaces.Services;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.CrossCutting.HealthChecks;
using WellMindApi.Infrastructure.Data;
using WellMindApi.Infrastructure.Data.Context;
using WellMindApi.Infrastructure.Data.Repositories;
using WellMindApi.Infrastructure.External.ML;
using WellMindApi.Infrastructure.External.Services;

namespace WellMindApi.Infrastructure;

/// <summary>
/// Extensões para configurar injeção de dependência da camada Infrastructure
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ====================================================================
        // DATABASE - Oracle
        // ====================================================================

        var connectionString = configuration.GetConnectionString("WellMindDatabase")
            ?? throw new InvalidOperationException("Connection string 'WellMindDatabase' not found");

        services.AddDbContext<WellMindDbContext>(options =>
        {
            options.UseOracle(connectionString, oracleOptions =>
            {
                oracleOptions.CommandTimeout(60);
            });

            // Habilitar logs detalhados em desenvolvimento
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // ====================================================================
        // UNIT OF WORK
        // ====================================================================

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ====================================================================
        // REPOSITORIES
        // ====================================================================

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IRegistroBemEstarRepository, RegistroBemEstarRepository>();
        services.AddScoped<IAlertaRepository, AlertaRepository>();
        services.AddScoped<IRecomendacaoRepository, RecomendacaoRepository>();
        services.AddScoped<ISessaoRepository, SessaoRepository>();
        services.AddScoped<ITransicaoCarreiraRepository, TransicaoCarreiraRepository>();

        // ====================================================================
        // EXTERNAL SERVICES
        // ====================================================================

        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<BurnoutPredictor>();

        // ====================================================================
        // HEALTH CHECKS
        // ====================================================================

        services.AddHealthChecks()
            .AddCheck("oracle_database",
                new OracleHealthCheck(connectionString),
                tags: new[] { "database", "oracle" })
            .AddCheck<MemoryHealthCheck>(
                "memory",
                tags: new[] { "memory" });

        // Configurar opções de Memory Health Check
        services.Configure<MemoryCheckOptions>(options =>
        {
            options.Threshold = 1024L * 1024L * 1024L; // 1 GB
        });

        // Health Checks UI (Dashboard)
        services.AddHealthChecksUI(setup =>
        {
            setup.SetEvaluationTimeInSeconds(30); // Avaliar a cada 30 segundos
            setup.MaximumHistoryEntriesPerEndpoint(50);
            setup.AddHealthCheckEndpoint("WellMind API", "/health");
        })
        .AddInMemoryStorage();

        return services;
    }

    /// <summary>
    /// Aplica migrations pendentes automaticamente (USE APENAS EM DESENVOLVIMENTO)
    /// </summary>
    public static async Task ApplyMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WellMindDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    /// <summary>
    /// Inicializa o preditor de burnout
    /// </summary>
    public static void InicializarBurnoutPredictor(this IServiceProvider services)
    {
        var predictor = services.GetRequiredService<BurnoutPredictor>();

        // Tentar carregar modelo existente
        predictor.CarregarModelo();

        // Se não existir, você pode treinar com dados históricos aqui
        // predictor.TreinarModelo(dadosTreinamento);
    }
}
