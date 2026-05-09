using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISalesOrderHeaderRepository
{
  Task<SalesOrderHeader?> GetByIdAsync(int salesOrderID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SalesOrderHeader>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SalesOrderHeader salesOrderHeader, CancellationToken cancellationToken = default);
  Task UpdateAsync(SalesOrderHeader salesOrderHeader, CancellationToken cancellationToken = default);
  Task DeleteAsync(int salesOrderID, CancellationToken cancellationToken = default);
}