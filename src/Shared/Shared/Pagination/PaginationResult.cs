namespace Shared.Pagination;

public class PaginationResult<T>
    (int pageIndex, int pageSize, long count, IEnumerable<T> items)
    where T : class
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public long Count { get; set; } = count;
    public IEnumerable<T> Items { get; set; } = items;
}
