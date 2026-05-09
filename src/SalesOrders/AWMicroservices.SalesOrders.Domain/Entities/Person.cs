namespace AWMicroservices.SalesOrders.Domain.Entities;

public class Person
{
  public int BusinessEntityID { get; private set; }
  public string PersonType { get; private set; }
  public bool NameStyle { get; private set; }
  public string? Title { get; private set; }
  public string FirstName { get; private set; }
  public string? MiddleName { get; private set; }
  public string LastName { get; private set; }
  public string? Suffix { get; private set; }
  public int EmailPromotion { get; private set; }
  public string? AdditionalContactInfo { get; private set; } // XML as string
  public string? Demographics { get; private set; } // XML as string
  public Guid Rowguid { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private Person() { } // For EF Core

  public Person(int businessEntityID, string personType, bool nameStyle, string? title, string firstName, string? middleName, string lastName, string? suffix, int emailPromotion, string? additionalContactInfo, string? demographics, Guid rowguid, DateTime modifiedDate)
  {
    BusinessEntityID = businessEntityID;
    PersonType = personType ?? throw new ArgumentNullException(nameof(personType));
    NameStyle = nameStyle;
    Title = title;
    FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
    MiddleName = middleName;
    LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
    Suffix = suffix;
    EmailPromotion = emailPromotion;
    AdditionalContactInfo = additionalContactInfo;
    Demographics = demographics;
    Rowguid = rowguid;
    ModifiedDate = modifiedDate;
  }
}