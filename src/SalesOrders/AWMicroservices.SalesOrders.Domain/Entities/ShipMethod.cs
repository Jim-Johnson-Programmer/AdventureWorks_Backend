namespace AWMicroservices.SalesOrders.Domain.Entities;

public class ShipMethod
{
  public int ShipMethodID { get; private set; }
  public string Name { get; private set; }
  public decimal ShipBase { get; private set; }
  public decimal ShipRate { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private ShipMethod() { } // For EF Core

  public ShipMethod(string name, decimal shipBase, decimal shipRate, Guid rowguid, DateTime modifiedDate)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    ShipBase = shipBase;
    ShipRate = shipRate;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}