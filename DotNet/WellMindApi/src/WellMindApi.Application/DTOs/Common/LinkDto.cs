namespace WellMindApi.Application.DTOs.Common;

/// <summary>
/// DTO para links HATEOAS
/// </summary>
public record LinkDto(
    string Href,
    string Rel,
    string Method
);
