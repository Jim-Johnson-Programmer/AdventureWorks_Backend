namespace AWMicroservices.SalesOrders.Domain.Entities;

public class CurrencyRate
{
  public int CurrencyRateID { get; private set; }
  public DateTime CurrencyRateDate { get; private set; }
  public string FromCurrencyCode { get; private set; }
  public string ToCurrencyCode { get; private set; }
  public decimal AverageRate { get; private set; }
  public decimal EndOfDayRate { get; private set; }
  public DateTime ModifiedDate { get; private set; }

  private CurrencyRate() { } // For EF Core

  public CurrencyRate(DateTime currencyRateDate, string fromCurrencyCode, string toCurrencyCode, decimal averageRate, decimal endOfDayRate, DateTime modifiedDate)
  {
    CurrencyRateDate = currencyRateDate;
    FromCurrencyCode = fromCurrencyCode ?? throw new ArgumentNullException(nameof(fromCurrencyCode));
    ToCurrencyCode = toCurrencyCode ?? throw new ArgumentNullException(nameof(toCurrencyCode));
    AverageRate = averageRate;
    EndOfDayRate = endOfDayRate;
    ModifiedDate = modifiedDate;
  }
}