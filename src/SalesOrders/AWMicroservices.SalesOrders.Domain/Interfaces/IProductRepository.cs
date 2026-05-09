using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface IProductRepository
{
  Task<Product?> GetByIdAsync(int productID, CancellationToken cancellationToken = default);
  Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(Product product, CancellationToken cancellationToken = default);
  Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
  Task DeleteAsync(int productID, CancellationToken cancellationToken = default);
}