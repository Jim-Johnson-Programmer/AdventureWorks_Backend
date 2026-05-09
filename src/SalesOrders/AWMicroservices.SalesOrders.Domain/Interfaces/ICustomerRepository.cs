using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ICustomerRepository
{
  Task<Customer?> GetByIdAsync(int customerID, CancellationToken cancellationToken = default);
  Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
  Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
  Task DeleteAsync(int customerID, CancellationToken cancellationToken = default);
}