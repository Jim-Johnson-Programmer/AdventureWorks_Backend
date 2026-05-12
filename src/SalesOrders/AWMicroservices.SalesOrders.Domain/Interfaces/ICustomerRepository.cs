using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ICustomerRepository
{
  Task<Customer?> GetByIdAsync(int customerID, CancellationToken cancellationToken = default);
  Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
  Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
  Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
  Task DeleteAsync(int customerID, CancellationToken cancellationToken = default);
  Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = null, CancellationToken cancellationToken = default);
}