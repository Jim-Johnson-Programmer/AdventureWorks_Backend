using MediatR;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Queries;

public record GetCustomerByIdQuery(int CustomerID) : IRequest<CustomerDto?>;
