using System.Net;
using System.Text.Json;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Api.Middlewares;

/// <summary>
/// Middleware global para tratamento de exceções
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case DomainException domainEx:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new
                {
                    error = "Erro de validação",
                    message = domainEx.Message,
                    type = "DomainException"
                });
                break;

            case InvalidOperationException invalidOpEx:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new
                {
                    error = "Operação inválida",
                    message = invalidOpEx.Message,
                    type = "InvalidOperationException"
                });
                break;

            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new
                {
                    error = "Acesso não autorizado",
                    message = "Você não tem permissão para acessar este recurso"
                });
                break;

            case KeyNotFoundException notFoundEx:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new
                {
                    error = "Recurso não encontrado",
                    message = notFoundEx.Message
                });
                break;

            default:
                result = JsonSerializer.Serialize(new
                {
                    error = "Erro interno do servidor",
                    message = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.",
                    type = exception.GetType().Name
                });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
