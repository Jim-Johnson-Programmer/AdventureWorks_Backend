using MediatR;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public record GetAllCustomersQuery(int PageNumber = 1, int PageSize = 50) : IRequest<IEnumerable<CustomerDto>>;
