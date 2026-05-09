namespace AWMicroservices.SalesOrders.Domain.Entities;

public class Customer
{
  public int CustomerID { get; private set; }
  public int? PersonID { get; private set; }
  public int? StoreID { get; private set; }
  public int? TerritoryID { get; private set; }
  public string AccountNumber { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private Customer() { } // For EF Core

  public Customer(int? personID, int? storeID, int? territoryID, string accountNumber, Guid rowguid, DateTime modifiedDate)
  {
    PersonID = personID;
    StoreID = storeID;
    TerritoryID = territoryID;
    AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}