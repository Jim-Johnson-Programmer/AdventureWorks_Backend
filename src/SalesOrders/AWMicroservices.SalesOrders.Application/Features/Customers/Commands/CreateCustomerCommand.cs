using MediatR;

namespace AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

public record CreateCustomerCommand(
    int? PersonID,
    int? StoreID,
    int? TerritoryID,
    string AccountNumber,
    Guid Rowguid,
    DateTime ModifiedDate
) : IRequest<int>;
