namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesOrderHeaderSalesReason
{
  public int SalesOrderID { get; private set; }
  public int SalesReasonID { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SalesOrderHeaderSalesReason() { } // For EF Core

  public SalesOrderHeaderSalesReason(int salesOrderID, int salesReasonID, DateTime modifiedDate)
  {
    SalesOrderID = salesOrderID;
    SalesReasonID = salesReasonID;
    ModifiedDate = modifiedDate;
  }
}