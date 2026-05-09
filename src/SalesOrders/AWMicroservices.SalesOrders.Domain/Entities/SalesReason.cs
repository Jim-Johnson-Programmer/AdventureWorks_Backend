namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SalesReason
{
  public int SalesReasonID { get; private set; }
  public string Name { get; private set; }
  public string ReasonType { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SalesReason() { } // For EF Core

  public SalesReason(string name, string reasonType, DateTime modifiedDate)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    ReasonType = reasonType ?? throw new ArgumentNullException(nameof(reasonType));
    ModifiedDate = modifiedDate;
  }
}