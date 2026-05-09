namespace AWMicroservices.SalesOrders.Domain.Entities;

public class Product
{
  public int ProductID { get; private set; }
  public string Name { get; private set; }
  public string ProductNumber { get; private set; }
  public bool MakeFlag { get; private set; }
  public bool FinishedGoodsFlag { get; private set; }
  public string? Color { get; private set; }
  public short SafetyStockLevel { get; private set; }
  public short ReorderPoint { get; private set; }
  public decimal StandardCost { get; private set; }
  public decimal ListPrice { get; private set; }
  public string? Size { get; private set; }
  public string? SizeUnitMeasureCode { get; private set; }
  public string? WeightUnitMeasureCode { get; private set; }
  public decimal? Weight { get; private set; }
  public int DaysToManufacture { get; private set; }
  public string? ProductLine { get; private set; }
  public string? Class { get; private set; }
  public string? Style { get; private set; }
  public int? ProductSubcategoryID { get; private set; }
  public int? ProductModelID { get; private set; }
  public DateTime SellStartDate { get; private set; }
  public DateTime? SellEndDate { get; private set; }
  public DateTime? DiscontinuedDate { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private Product() { } // For EF Core

  public Product(string name, string productNumber, bool makeFlag, bool finishedGoodsFlag, string? color, short safetyStockLevel, short reorderPoint, decimal standardCost, decimal listPrice, string? size, string? sizeUnitMeasureCode, string? weightUnitMeasureCode, decimal? weight, int daysToManufacture, string? productLine, string? @class, string? style, int? productSubcategoryID, int? productModelID, DateTime sellStartDate, DateTime? sellEndDate, DateTime? discontinuedDate, Guid rowguid, DateTime modifiedDate)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    ProductNumber = productNumber ?? throw new ArgumentNullException(nameof(productNumber));
    MakeFlag = makeFlag;
    FinishedGoodsFlag = finishedGoodsFlag;
    Color = color;
    SafetyStockLevel = safetyStockLevel;
    ReorderPoint = reorderPoint;
    StandardCost = standardCost;
    ListPrice = listPrice;
    Size = size;
    SizeUnitMeasureCode = sizeUnitMeasureCode;
    WeightUnitMeasureCode = weightUnitMeasureCode;
    Weight = weight;
    DaysToManufacture = daysToManufacture;
    ProductLine = productLine;
    Class = @class;
    Style = style;
    ProductSubcategoryID = productSubcategoryID;
    ProductModelID = productModelID;
    SellStartDate = sellStartDate;
    SellEndDate = sellEndDate;
    DiscontinuedDate = discontinuedDate;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}