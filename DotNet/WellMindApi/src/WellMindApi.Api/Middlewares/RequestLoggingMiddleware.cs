using System.Diagnostics;

namespace WellMindApi.Api.Middlewares;

/// <summary>
/// Middleware para logging detalhado de requisições
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();

        // Log da requisição
        _logger.LogInformation(
            "REQUEST [{RequestId}] {Method} {Path} - Usuario: {User}",
            requestId,
            context.Request.Method,
            context.Request.Path,
            context.User?.Identity?.Name ?? "Anônimo");

        try
        {
            await _next(context);
            stopwatch.Stop();

            // Log da resposta
            _logger.LogInformation(
                "RESPONSE [{RequestId}] {StatusCode} - {ElapsedMs}ms",
                requestId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "ERROR [{RequestId}] {Message} - {ElapsedMs}ms",
                requestId,
                ex.Message,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
