namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesOrder
{
  public int Id { get; private set; }
  public string OrderNumber { get; private set; }
  public DateTime OrderDate { get; private set; }
  public decimal TotalAmount { get; private set; }

  private SalesOrder() { } // For EF Core

  public SalesOrder(int id, string orderNumber, DateTime orderDate, decimal totalAmount)
  {
    if (string.IsNullOrWhiteSpace(orderNumber))
      throw new ArgumentException("Order number cannot be empty.", nameof(orderNumber));
    if (totalAmount < 0)
      throw new ArgumentException("Total amount cannot be negative.", nameof(totalAmount));

    Id = id;
    OrderNumber = orderNumber;
    OrderDate = orderDate;
    TotalAmount = totalAmount;
  }

  public void UpdateTotal(decimal newTotal)
  {
    if (newTotal < 0) throw new ArgumentException("Total amount cannot be negative.");
    TotalAmount = newTotal;
  }
}