namespace AWMicroservices.SalesOrders.Domain.Entities;

public class SpecialOfferProduct
{
  public int SpecialOfferID { get; private set; }
  public int ProductID { get; private set; }
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private SpecialOfferProduct() { } // For EF Core

  public SpecialOfferProduct(int specialOfferID, int productID, Guid rowguid, DateTime modifiedDate)
  {
    SpecialOfferID = specialOfferID;
    ProductID = productID;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}