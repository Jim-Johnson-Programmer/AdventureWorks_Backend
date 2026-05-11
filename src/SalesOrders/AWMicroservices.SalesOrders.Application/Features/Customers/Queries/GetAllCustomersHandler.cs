using MediatR;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public class GetAllCustomersHandler(ICustomerRepository repository) : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
  public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
  {
    var customers = await repository.GetAllAsync(cancellationToken);
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
