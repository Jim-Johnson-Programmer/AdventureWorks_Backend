using MediatR;
using AWMicroservices.SalesOrders.Domain.Entities;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

public class UpdateCustomerHandler(ICustomerRepository repository) : IRequestHandler<UpdateCustomerCommand>
{
  public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
  {
    var customer = await repository.GetByIdAsync(request.CustomerID, cancellationToken);
    if (customer is null) throw new Exception($"Customer {request.CustomerID} not found");
    // Update properties
    typeof(Customer).GetProperty("PersonID")?.SetValue(customer, request.PersonID);
    typeof(Customer).GetProperty("StoreID")?.SetValue(customer, request.StoreID);
    typeof(Customer).GetProperty("TerritoryID")?.SetValue(customer, request.TerritoryID);
    typeof(Customer).GetProperty("AccountNumber")?.SetValue(customer, request.AccountNumber);
    typeof(Customer).GetProperty("Rowguid")?.SetValue(customer, request.Rowguid);
    typeof(Customer).GetProperty("ModifiedDate")?.SetValue(customer, request.ModifiedDate);
    await repository.UpdateAsync(customer, cancellationToken);
  }
}
