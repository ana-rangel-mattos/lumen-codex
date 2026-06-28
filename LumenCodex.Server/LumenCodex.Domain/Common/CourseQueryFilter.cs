namespace LumenCodex.Domain.Common;

public record CourseQueryFilter(
    string? SortBy,
    string? SearchTerm,
    int PageNumber = 1,
    int PageSize = 10
);