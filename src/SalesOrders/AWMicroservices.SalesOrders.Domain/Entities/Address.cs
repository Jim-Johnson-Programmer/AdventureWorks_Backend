namespace AWMicroservices.SalesOrders.Domain.Entities;

public class Address
{
  public int AddressID { get; private set; }
  public string AddressLine1 { get; private set; }
  public string? AddressLine2 { get; private set; }
  public string City { get; private set; }
  public int StateProvinceID { get; private set; }
  public string PostalCode { get; private set; }
  public string? SpatialLocation { get; private set; } // Geography as string
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private Address() { } // For EF Core

  public Address(string addressLine1, string? addressLine2, string city, int stateProvinceID, string postalCode, string? spatialLocation, Guid rowguid, DateTime modifiedDate)
  {
    AddressLine1 = addressLine1 ?? throw new ArgumentNullException(nameof(addressLine1));
    AddressLine2 = addressLine2;
    City = city ?? throw new ArgumentNullException(nameof(city));
    StateProvinceID = stateProvinceID;
    PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
    SpatialLocation = spatialLocation;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}