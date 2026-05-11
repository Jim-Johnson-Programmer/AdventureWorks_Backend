using MediatR;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

public class DeleteCustomerHandler(ICustomerRepository repository) : IRequestHandler<DeleteCustomerCommand>
{
  public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
  {
    await repository.DeleteAsync(request.CustomerID, cancellationToken);
  }
}
