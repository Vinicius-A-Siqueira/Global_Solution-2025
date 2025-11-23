using WellMindApi.Application.DTOs.Common;

namespace WellMindApi.Infrastructure.CrossCutting.Helpers;

/// <summary>
/// Helper para gerar links HATEOAS
/// </summary>
public static class HateoasHelper
{
    /// <summary>
    /// Adiciona links HATEOAS para recurso de Usuário
    /// </summary>
    public static Dictionary<string, LinkDto> GerarLinksUsuario(int idUsuario, string baseUrl)
    {
        return new Dictionary<string, LinkDto>
        {
            { "self", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}", "self", "GET") },
            { "update", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}", "update_usuario", "PUT") },
            { "delete", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}", "delete_usuario", "DELETE") },
            { "registros", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}/registros", "registros_bemestar", "GET") },
            { "alertas", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}/alertas", "alertas_usuario", "GET") },
            { "analise", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}/analise", "analise_bemestar", "GET") }
        };
    }

    /// <summary>
    /// Adiciona links HATEOAS para recurso de Alerta
    /// </summary>
    public static Dictionary<string, LinkDto> GerarLinksAlerta(int idAlerta, string status, string baseUrl)
    {
        var links = new Dictionary<string, LinkDto>
        {
            { "self", new LinkDto($"{baseUrl}/api/v1/alertas/{idAlerta}", "self", "GET") }
        };

        // Links condicionais baseados no status
        if (status == "PENDENTE")
        {
            links.Add("analisar", new LinkDto(
                $"{baseUrl}/api/v1/alertas/{idAlerta}/status",
                "colocar_em_analise",
                "PATCH"));

            links.Add("resolver", new LinkDto(
                $"{baseUrl}/api/v1/alertas/{idAlerta}/status",
                "resolver_alerta",
                "PATCH"));
        }
        else if (status == "EM_ANALISE")
        {
            links.Add("resolver", new LinkDto(
                $"{baseUrl}/api/v1/alertas/{idAlerta}/status",
                "resolver_alerta",
                "PATCH"));

            links.Add("cancelar", new LinkDto(
                $"{baseUrl}/api/v1/alertas/{idAlerta}/status",
                "cancelar_alerta",
                "PATCH"));
        }

        return links;
    }

    /// <summary>
    /// Adiciona links HATEOAS para recurso de Registro
    /// </summary>
    public static Dictionary<string, LinkDto> GerarLinksRegistro(int idRegistro, int idUsuario, string baseUrl)
    {
        return new Dictionary<string, LinkDto>
        {
            { "self", new LinkDto($"{baseUrl}/api/v1/registros/{idRegistro}", "self", "GET") },
            { "usuario", new LinkDto($"{baseUrl}/api/v1/usuarios/{idUsuario}", "usuario_proprietario", "GET") },
            { "update", new LinkDto($"{baseUrl}/api/v1/registros/{idRegistro}", "update_registro", "PUT") },
            { "delete", new LinkDto($"{baseUrl}/api/v1/registros/{idRegistro}", "delete_registro", "DELETE") }
        };
    }

    /// <summary>
    /// Gera link de paginação
    /// </summary>
    public static Dictionary<string, LinkDto> GerarLinksPaginacao(
        string baseUrl,
        string recurso,
        int currentPage,
        int totalPages,
        int pageSize)
    {
        var links = new Dictionary<string, LinkDto>
        {
            { "self", new LinkDto($"{baseUrl}/api/v1/{recurso}?page={currentPage}&pageSize={pageSize}", "self", "GET") },
            { "first", new LinkDto($"{baseUrl}/api/v1/{recurso}?page=1&pageSize={pageSize}", "first_page", "GET") },
            { "last", new LinkDto($"{baseUrl}/api/v1/{recurso}?page={totalPages}&pageSize={pageSize}", "last_page", "GET") }
        };

        if (currentPage > 1)
        {
            links.Add("previous", new LinkDto(
                $"{baseUrl}/api/v1/{recurso}?page={currentPage - 1}&pageSize={pageSize}",
                "previous_page",
                "GET"));
        }

        if (currentPage < totalPages)
        {
            links.Add("next", new LinkDto(
                $"{baseUrl}/api/v1/{recurso}?page={currentPage + 1}&pageSize={pageSize}",
                "next_page",
                "GET"));
        }

        return links;
    }
}
