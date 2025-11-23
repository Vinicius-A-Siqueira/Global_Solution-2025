namespace WellMindApi.Api.Configuration;

/// <summary>
/// Configuração de CORS
/// </summary>
public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy("Production", builder =>
            {
                builder
                    .WithOrigins(
                        "https://wellmind.com.br",
                        "https://app.wellmind.com.br")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
