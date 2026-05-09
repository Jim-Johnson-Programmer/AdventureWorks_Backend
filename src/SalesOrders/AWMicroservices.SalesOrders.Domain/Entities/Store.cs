namespace AWMicroservices.SalesOrders.Domain.Entities;

public class Store
{
  public int BusinessEntityID { get; private set; }
  public string Name { get; private set; }
  public int? SalesPersonID { get; private set; }
  public string? Demographics { get; private set; } // XML as string
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private Store() { } // For EF Core

  public Store(int businessEntityID, string name, int? salesPersonID, string? demographics, Guid rowguid, DateTime modifiedDate)
  {
    BusinessEntityID = businessEntityID;
    Name = name ?? throw new ArgumentNullException(nameof(name));
    SalesPersonID = salesPersonID;
    Demographics = demographics;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}