namespace AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;

public class PagedResult<T>
{
  public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
  public int TotalCount { get; set; }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public string? SortBy { get; set; }
  public string? SortDirection { get; set; } // "asc" or "desc"
}
