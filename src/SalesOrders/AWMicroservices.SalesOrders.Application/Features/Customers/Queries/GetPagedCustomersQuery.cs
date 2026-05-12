using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using MediatR;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public record GetPagedCustomersQuery(
    int PageNumber = 1,
    int PageSize = 50,
    string? SortBy = null,
    string? SortDirection = null // "asc" or "desc"
) : IRequest<PagedResult<CustomerDto>>;
