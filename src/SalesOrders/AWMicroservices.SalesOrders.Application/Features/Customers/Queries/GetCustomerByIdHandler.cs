using MediatR;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public class GetCustomerByIdHandler(ICustomerRepository repository) : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
  public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
  {
    var customer = await repository.GetByIdAsync(request.CustomerID, cancellationToken);
    if (customer is null) return null;
    return new CustomerDto(
        customer.CustomerID,
        customer.PersonID,
        customer.StoreID,
        customer.TerritoryID,
        customer.AccountNumber,
        customer.Rowguid,
        customer.ModifiedDate
    );
  }
}
