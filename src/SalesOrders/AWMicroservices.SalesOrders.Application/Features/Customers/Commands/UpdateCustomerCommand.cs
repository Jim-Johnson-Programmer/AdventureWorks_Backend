using MediatR;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

public record UpdateCustomerCommand(
    int CustomerID,
    int? PersonID,
    int? StoreID,
    int? TerritoryID,
    string AccountNumber,
    Guid Rowguid,
    DateTime ModifiedDate
) : IRequest;
