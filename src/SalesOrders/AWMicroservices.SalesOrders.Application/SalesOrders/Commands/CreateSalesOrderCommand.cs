// using MediatR;
// using AWMicroservices.SalesOrders.Domain.Entities;
// using AWMicroservices.SalesOrders.Domain.Interfaces;

// namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

// public record CreateSalesOrderCommand(string OrderNumber, DateTime OrderDate, decimal TotalAmount) : IRequest<int>;

// public class CreateSalesOrderHandler(ISalesOrderRepository repository)
//     : IRequestHandler<CreateSalesOrderCommand, int>
// {
//   public async Task<int> Handle(CreateSalesOrderCommand request, CancellationToken ct)
//   {
//     var salesOrder = new SalesOrder(request.OrderNumber, request.OrderDate, request.TotalAmount);
//     await repository.AddAsync(salesOrder, ct);
//     return salesOrder.Id;
//   }
// }