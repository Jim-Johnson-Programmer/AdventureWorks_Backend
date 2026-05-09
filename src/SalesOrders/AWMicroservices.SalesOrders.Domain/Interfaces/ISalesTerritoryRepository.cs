using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISalesTerritoryRepository
{
  Task<SalesTerritory?> GetByIdAsync(int territoryID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SalesTerritory>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SalesTerritory salesTerritory, CancellationToken cancellationToken = default);
  Task UpdateAsync(SalesTerritory salesTerritory, CancellationToken cancellationToken = default);
  Task DeleteAsync(int territoryID, CancellationToken cancellationToken = default);
}