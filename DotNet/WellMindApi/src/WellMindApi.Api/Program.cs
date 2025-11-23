using Serilog;
using WellMindApi.Api.Configuration;
using WellMindApi.Api.Middlewares;
using WellMindApi.Application;
using WellMindApi.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURAÇÃO DE LOGGING - SERILOG
// ============================================================================

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/wellmind-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("🚀 Iniciando WellMind API...");

// ============================================================================
// CONFIGURAÇÃO DE SERVIÇOS
// ============================================================================

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase
        options.JsonSerializerOptions.WriteIndented = true;
    });

// API Versioning
builder.Services.AddApiVersioningConfiguration();

// Swagger/OpenAPI
builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

// CORS
builder.Services.AddCorsConfiguration();

// JWT Authentication
builder.Services.AddJwtConfiguration(builder.Configuration);

// Application Layer (Use Cases, Validators, Mappings)
builder.Services.AddApplicationServices();

// Infrastructure Layer (DbContext, Repositories, External Services)
builder.Services.AddInfrastructureServices(builder.Configuration);

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// ============================================================================
// BUILD DA APLICAÇÃO
// ============================================================================

var app = builder.Build();

// ============================================================================
// CONFIGURAÇÃO DO PIPELINE HTTP
// ============================================================================

// Middlewares customizados
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Swagger (disponível em todos os ambientes para fins educacionais)
app.UseSwaggerConfiguration(app.Services.GetRequiredService<Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider>());

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    Log.Information("🌐 CORS: Permitindo todas as origens (Development)");
}
else
{
    app.UseCors("Production");
    Log.Information("🌐 CORS: Política de produção ativada");
}

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Health Checks UI
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-ui-api";
});

Log.Information("🏥 Health Checks configurados:");
Log.Information("   - /health (status completo)");
Log.Information("   - /health/ready (readiness)");
Log.Information("   - /health/live (liveness)");
Log.Information("   - /health-ui (dashboard)");

// Controllers
app.MapControllers();

// ============================================================================
// INICIALIZAÇÃO (DESENVOLVIMENTO)
// ============================================================================

if (app.Environment.IsDevelopment())
{
    // Aplicar migrations automaticamente em desenvolvimento
    try
    {
        await app.Services.ApplyMigrationsAsync();
        Log.Information("✅ Migrations aplicadas com sucesso");
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "⚠️ Erro ao aplicar migrations: {Message}", ex.Message);
    }

    // Inicializar ML.NET Predictor
    try
    {
        app.Services.InicializarBurnoutPredictor();
        Log.Information("🤖 Preditor de burnout inicializado");
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "⚠️ Erro ao inicializar preditor: {Message}", ex.Message);
    }
}

// ============================================================================
// EXECUÇÃO
// ============================================================================

try
{
    Log.Information("✅ WellMind API iniciada com sucesso!");
    Log.Information("🌍 Ambiente: {Environment}", app.Environment.EnvironmentName);
    Log.Information("📚 Swagger: {Url}", app.Environment.IsDevelopment() ? "https://localhost:7000" : "Não disponível");
    Log.Information("💚 Health UI: {Url}/health-ui", app.Environment.IsDevelopment() ? "https://localhost:7000" : "");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Erro fatal ao iniciar a aplicação");
    throw;
}
finally
{
    Log.Information("🛑 WellMind API encerrada");
    Log.CloseAndFlush();
}
