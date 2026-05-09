namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesTerritory
{
  public int TerritoryID { get; private set; }
  public string Name { get; private set; }
  public string CountryRegionCode { get; private set; }
  public string Group { get; private set; }
  public decimal SalesYTD { get; private set; }
  public decimal SalesLastYear { get; private set; }
  public decimal CostYTD { get; private set; }
  public decimal CostLastYear { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SalesTerritory() { } // For EF Core

  public SalesTerritory(string name, string countryRegionCode, string group, decimal salesYTD, decimal salesLastYear, decimal costYTD, decimal costLastYear, Guid rowguid, DateTime modifiedDate)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    CountryRegionCode = countryRegionCode ?? throw new ArgumentNullException(nameof(countryRegionCode));
    Group = group ?? throw new ArgumentNullException(nameof(group));
    SalesYTD = salesYTD;
    SalesLastYear = salesLastYear;
    CostYTD = costYTD;
    CostLastYear = costLastYear;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}