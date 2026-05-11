using MediatR;
using AWMicroservices.SalesOrders.Domain.Entities;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

public class CreateCustomerHandler(ICustomerRepository repository) : IRequestHandler<CreateCustomerCommand, int>
{
  public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
  {
    var customer = new Customer(
        request.PersonID,
        request.StoreID,
        request.TerritoryID,
        request.AccountNumber,
        request.Rowguid,
        request.ModifiedDate
    );
    await repository.AddAsync(customer, cancellationToken);
    return customer.CustomerID;
  }
}
