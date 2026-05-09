using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface IPersonRepository
{
  Task<Person?> GetByIdAsync(int businessEntityID, CancellationToken cancellationToken = default);
  Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(Person person, CancellationToken cancellationToken = default);
  Task UpdateAsync(Person person, CancellationToken cancellationToken = default);
  Task DeleteAsync(int businessEntityID, CancellationToken cancellationToken = default);
}