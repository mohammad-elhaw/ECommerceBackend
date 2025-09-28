namespace Shared.Pagination;
public class PaginatedResult<TEntity>
    (int pageNumber, int pageSize, long totalRecords, long totalPages, List<TEntity> items)
    where TEntity : class

{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public long TotalRecords { get; } = totalRecords;
    public long TotalPages { get; } = totalPages;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public List<TEntity> Items { get; } = items;
}
