using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISalesReasonRepository
{
  Task<SalesReason?> GetByIdAsync(int salesReasonID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SalesReason>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SalesReason salesReason, CancellationToken cancellationToken = default);
  Task UpdateAsync(SalesReason salesReason, CancellationToken cancellationToken = default);
  Task DeleteAsync(int salesReasonID, CancellationToken cancellationToken = default);
}