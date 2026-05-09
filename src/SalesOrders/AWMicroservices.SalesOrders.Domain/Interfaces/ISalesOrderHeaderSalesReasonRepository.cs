using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISalesOrderHeaderSalesReasonRepository
{
  Task<SalesOrderHeaderSalesReason?> GetByIdAsync(int salesOrderID, int salesReasonID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SalesOrderHeaderSalesReason>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SalesOrderHeaderSalesReason salesOrderHeaderSalesReason, CancellationToken cancellationToken = default);
  Task UpdateAsync(SalesOrderHeaderSalesReason salesOrderHeaderSalesReason, CancellationToken cancellationToken = default);
  Task DeleteAsync(int salesOrderID, int salesReasonID, CancellationToken cancellationToken = default);
}