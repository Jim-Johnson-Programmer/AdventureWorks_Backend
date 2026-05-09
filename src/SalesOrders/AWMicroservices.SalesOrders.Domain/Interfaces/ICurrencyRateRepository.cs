using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ICurrencyRateRepository
{
  Task<CurrencyRate?> GetByIdAsync(int currencyRateID, CancellationToken cancellationToken = default);
  Task<IEnumerable<CurrencyRate>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(CurrencyRate currencyRate, CancellationToken cancellationToken = default);
  Task UpdateAsync(CurrencyRate currencyRate, CancellationToken cancellationToken = default);
  Task DeleteAsync(int currencyRateID, CancellationToken cancellationToken = default);
}