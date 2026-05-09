using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISalesPersonRepository
{
  Task<SalesPerson?> GetByIdAsync(int businessEntityID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SalesPerson>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SalesPerson salesPerson, CancellationToken cancellationToken = default);
  Task UpdateAsync(SalesPerson salesPerson, CancellationToken cancellationToken = default);
  Task DeleteAsync(int businessEntityID, CancellationToken cancellationToken = default);
}