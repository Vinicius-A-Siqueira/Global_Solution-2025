namespace WellMindApi.Application.DTOs.Common;

/// <summary>
/// DTO para resultados paginados
/// </summary>
public record PagedResultDto<T>
{
    public List<T> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageSize { get; init; }
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }

    public static PagedResultDto<T> Create(
        List<T> items,
        int totalCount,
        int pageNumber,
        int pageSize)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            HasPrevious = pageNumber > 1,
            HasNext = pageNumber < totalPages
        };
    }
}
