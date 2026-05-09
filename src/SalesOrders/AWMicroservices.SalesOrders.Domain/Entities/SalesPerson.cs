namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesPerson
{
  public int BusinessEntityID { get; private set; }
  public int? TerritoryID { get; private set; }
  public decimal? SalesQuota { get; private set; }
  public decimal Bonus { get; private set; }
  public decimal CommissionPct { get; private set; }
  public decimal SalesYTD { get; private set; }
  public decimal SalesLastYear { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SalesPerson() { } // For EF Core

  public SalesPerson(int businessEntityID, int? territoryID, decimal? salesQuota, decimal bonus, decimal commissionPct, decimal salesYTD, decimal salesLastYear, Guid rowguid, DateTime modifiedDate)
  {
    BusinessEntityID = businessEntityID;
    TerritoryID = territoryID;
    SalesQuota = salesQuota;
    Bonus = bonus;
    CommissionPct = commissionPct;
    SalesYTD = salesYTD;
    SalesLastYear = salesLastYear;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}