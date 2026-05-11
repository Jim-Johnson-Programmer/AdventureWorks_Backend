// using MediatR;
// using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
// using AWMicroservices.SalesOrders.Domain.Interfaces;

// namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

// public record GetSalesOrderByIdQuery(int Id) : IRequest<SalesOrderDto?>;

// public class GetSalesOrderByIdHandler(ISalesOrderRepository repository)
//     : IRequestHandler<GetSalesOrderByIdQuery, SalesOrderDto?>
// {
//   private readonly ISalesOrderRepository repository = repository;

//   public async Task<SalesOrderDto?> Handle(GetSalesOrderByIdQuery request, CancellationToken ct)
//   {
//     // var salesOrder = await repository.GetByIdAsync(request.Id, ct);
//     // if (salesOrder is null) return null;
//     return new SalesOrderDto(1, "22222", DateTime.Now, 123.45m);
//   }
// }