using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface IAddressRepository
{
  Task<Address?> GetByIdAsync(int addressID, CancellationToken cancellationToken = default);
  Task<IEnumerable<Address>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(Address address, CancellationToken cancellationToken = default);
  Task UpdateAsync(Address address, CancellationToken cancellationToken = default);
  Task DeleteAsync(int addressID, CancellationToken cancellationToken = default);
}