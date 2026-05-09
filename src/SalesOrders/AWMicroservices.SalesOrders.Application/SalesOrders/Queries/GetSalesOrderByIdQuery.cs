using MediatR;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public record GetSalesOrderByIdQuery(int Id) : IRequest<SalesOrderDto?>;

public class GetSalesOrderByIdHandler(ISalesOrderRepository repository)
    : IRequestHandler<GetSalesOrderByIdQuery, SalesOrderDto?>
{
  public async Task<SalesOrderDto?> Handle(GetSalesOrderByIdQuery request, CancellationToken ct)
  {
    var salesOrder = await repository.GetByIdAsync(request.Id, ct);
    if (salesOrder is null) return null;
    return new SalesOrderDto(salesOrder.Id, salesOrder.OrderNumber, salesOrder.OrderDate, salesOrder.TotalAmount);
  }
}