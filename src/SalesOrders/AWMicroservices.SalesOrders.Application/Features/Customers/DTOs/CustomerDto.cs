namespace AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;

public record CustomerDto(
    int CustomerID,
    int? PersonID,
    int? StoreID,
    int? TerritoryID,
    string AccountNumber,
    Guid Rowguid,
    DateTime ModifiedDate
);
