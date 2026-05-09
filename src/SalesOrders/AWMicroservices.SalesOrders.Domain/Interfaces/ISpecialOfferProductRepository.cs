using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface ISpecialOfferProductRepository
{
  Task<SpecialOfferProduct?> GetByIdAsync(int specialOfferID, int productID, CancellationToken cancellationToken = default);
  Task<IEnumerable<SpecialOfferProduct>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(SpecialOfferProduct specialOfferProduct, CancellationToken cancellationToken = default);
  Task UpdateAsync(SpecialOfferProduct specialOfferProduct, CancellationToken cancellationToken = default);
  Task DeleteAsync(int specialOfferID, int productID, CancellationToken cancellationToken = default);
}