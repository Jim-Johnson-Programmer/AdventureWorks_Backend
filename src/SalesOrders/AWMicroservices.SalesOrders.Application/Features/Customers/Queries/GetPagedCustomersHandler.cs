using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using AWMicroservices.SalesOrders.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public class GetPagedCustomersHandler : IRequestHandler<GetPagedCustomersQuery, PagedResult<CustomerDto>>
{
  private readonly ICustomerRepository repository;
  private readonly ILogger<GetPagedCustomersHandler> logger;

  public GetPagedCustomersHandler(ICustomerRepository repository, ILogger<GetPagedCustomersHandler> logger)
  {
    this.repository = repository;
    this.logger = logger;
  }

  public async Task<PagedResult<CustomerDto>> Handle(GetPagedCustomersQuery request, CancellationToken cancellationToken)
  {
    logger.LogInformation($"Handling GetPagedCustomersQuery: page {request.PageNumber}, size {request.PageSize}, sortBy: {request.SortBy}, sortDirection: {request.SortDirection}");
    var (items, totalCount) = await repository.GetPagedAsync(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection, cancellationToken);
    return new PagedResult<CustomerDto>
    {
      Items = items.Select(customer => new CustomerDto(
          customer.CustomerID,
          customer.PersonID,
          customer.StoreID,
          customer.TerritoryID,
          customer.AccountNumber,
          customer.Rowguid,
          customer.ModifiedDate
      )),
      TotalCount = totalCount,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      SortBy = request.SortBy,
      SortDirection = request.SortDirection
    };
  }
}
