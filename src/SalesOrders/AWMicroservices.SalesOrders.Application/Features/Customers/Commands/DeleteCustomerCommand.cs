using MediatR;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

public record DeleteCustomerCommand(int CustomerID) : IRequest;
