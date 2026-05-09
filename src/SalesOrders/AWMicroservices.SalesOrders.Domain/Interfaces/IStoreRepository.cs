using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface IStoreRepository
{
  Task<Store?> GetByIdAsync(int businessEntityID, CancellationToken cancellationToken = default);
  Task<IEnumerable<Store>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(Store store, CancellationToken cancellationToken = default);
  Task UpdateAsync(Store store, CancellationToken cancellationToken = default);
  Task DeleteAsync(int businessEntityID, CancellationToken cancellationToken = default);
}