namespace AWMicroservices.SalesOrders.Domain.Entities;

public class CreditCard
{
  public int CreditCardID { get; private set; }
  public string CardType { get; private set; }
  public string CardNumber { get; private set; }
  public byte ExpMonth { get; private set; }
  public short ExpYear { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private CreditCard() { } // For EF Core

  public CreditCard(string cardType, string cardNumber, byte expMonth, short expYear, DateTime modifiedDate)
  {
    CardType = cardType ?? throw new ArgumentNullException(nameof(cardType));
    CardNumber = cardNumber ?? throw new ArgumentNullException(nameof(cardNumber));
    ExpMonth = expMonth;
    ExpYear = expYear;
    ModifiedDate = modifiedDate;
  }
}