namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesOrderDetail
{
  public int SalesOrderID { get; private set; }
  public int SalesOrderDetailID { get; private set; }
  public string? CarrierTrackingNumber { get; private set; }
  public short OrderQty { get; private set; }
  public int ProductID { get; private set; }
  public int SpecialOfferID { get; private set; }
  public decimal UnitPrice { get; private set; }
  public decimal UnitPriceDiscount { get; private set; }
  public decimal LineTotal { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SalesOrderDetail() { } // For EF Core

  public SalesOrderDetail(int salesOrderID, string? carrierTrackingNumber, short orderQty, int productID, int specialOfferID, decimal unitPrice, decimal unitPriceDiscount, decimal lineTotal, Guid rowguid, DateTime modifiedDate)
  {
    SalesOrderID = salesOrderID;
    CarrierTrackingNumber = carrierTrackingNumber;
    OrderQty = orderQty;
    ProductID = productID;
    SpecialOfferID = specialOfferID;
    UnitPrice = unitPrice;
    UnitPriceDiscount = unitPriceDiscount;
    LineTotal = lineTotal;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}