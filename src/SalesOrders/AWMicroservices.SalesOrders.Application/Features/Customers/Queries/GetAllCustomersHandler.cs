using MediatR;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using AWMicroservices.SalesOrders.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
  private readonly ICustomerRepository repository;
  private readonly ILogger<GetAllCustomersHandler> logger;

  public GetAllCustomersHandler(ICustomerRepository repository, ILogger<GetAllCustomersHandler> logger)
  {
    this.repository = repository;
    this.logger = logger;
  }

  public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
  {
    this.logger.LogInformation("Mediatr Handler for GetAllCustomersQuery");
    var customers = await repository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

    this.logger.LogDebug("Retrieved {CustomerCount} customers from repository", customers.Count());
    return customers.Select(customer => new CustomerDto(
        customer.CustomerID,
        customer.PersonID,
        customer.StoreID,
        customer.TerritoryID,
        customer.AccountNumber,
        customer.Rowguid,
        customer.ModifiedDate
    ));
  }
}
