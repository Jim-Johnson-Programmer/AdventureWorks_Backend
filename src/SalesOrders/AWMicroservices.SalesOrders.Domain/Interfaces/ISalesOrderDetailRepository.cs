using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISalesOrderDetailRepository
{
  Task<SalesOrderDetail?> GetByIdAsync(int salesOrderID, int salesOrderDetailID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SalesOrderDetail>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SalesOrderDetail salesOrderDetail, CancellationToken cancellationToken = default);
  Task UpdateAsync(SalesOrderDetail salesOrderDetail, CancellationToken cancellationToken = default);
  Task DeleteAsync(int salesOrderID, int salesOrderDetailID, CancellationToken cancellationToken = default);
}