namespace AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;

public record SalesOrderDto(int Id, string OrderNumber, DateTime OrderDate, decimal TotalAmount);