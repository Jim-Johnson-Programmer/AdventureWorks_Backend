namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesOrderHeader
{
  public int SalesOrderID { get; private set; }
  public byte RevisionNumber { get; private set; }
  public DateTime OrderDate { get; private set; }
  public DateTime DueDate { get; private set; }
  public DateTime? ShipDate { get; private set; }
  public byte Status { get; private set; }
  public bool OnlineOrderFlag { get; private set; }
  public string SalesOrderNumber { get; private set; }
  public string? PurchaseOrderNumber { get; private set; }
  public string? AccountNumber { get; private set; }
  public int CustomerID { get; private set; }
  public int? SalesPersonID { get; private set; }
  public int? TerritoryID { get; private set; }
  public int BillToAddressID { get; private set; }
  public int ShipToAddressID { get; private set; }
  public int ShipMethodID { get; private set; }
  public int? CreditCardID { get; private set; }
  public string? CreditCardApprovalCode { get; private set; }
  public int? CurrencyRateID { get; private set; }
  public decimal SubTotal { get; private set; }
  public decimal TaxAmt { get; private set; }
  public decimal Freight { get; private set; }
  public decimal TotalDue { get; private set; }
  public string? Comment { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SalesOrderHeader() { } // For EF Core

  public SalesOrderHeader(byte revisionNumber, DateTime orderDate, DateTime dueDate, DateTime? shipDate, byte status, bool onlineOrderFlag, string salesOrderNumber, string? purchaseOrderNumber, string? accountNumber, int customerID, int? salesPersonID, int? territoryID, int billToAddressID, int shipToAddressID, int shipMethodID, int? creditCardID, string? creditCardApprovalCode, int? currencyRateID, decimal subTotal, decimal taxAmt, decimal freight, decimal totalDue, string? comment, Guid rowguid, DateTime modifiedDate)
  {
    RevisionNumber = revisionNumber;
    OrderDate = orderDate;
    DueDate = dueDate;
    ShipDate = shipDate;
    Status = status;
    OnlineOrderFlag = onlineOrderFlag;
    SalesOrderNumber = salesOrderNumber ?? throw new ArgumentNullException(nameof(salesOrderNumber));
    PurchaseOrderNumber = purchaseOrderNumber;
    AccountNumber = accountNumber;
    CustomerID = customerID;
    SalesPersonID = salesPersonID;
    TerritoryID = territoryID;
    BillToAddressID = billToAddressID;
    ShipToAddressID = shipToAddressID;
    ShipMethodID = shipMethodID;
    CreditCardID = creditCardID;
    CreditCardApprovalCode = creditCardApprovalCode;
    CurrencyRateID = currencyRateID;
    SubTotal = subTotal;
    TaxAmt = taxAmt;
    Freight = freight;
    TotalDue = totalDue;
    Comment = comment;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}