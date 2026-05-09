using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Domain.Interfaces;

public interface IShipMethodRepository
{
  Task<ShipMethod?> GetByIdAsync(int shipMethodID, CancellationToken cancellationToken = default);
  Task<IEnumerable<ShipMethod>> GetAllAsync(CancellationToken cancellationToken = default);
  Task AddAsync(ShipMethod shipMethod, CancellationToken cancellationToken = default);
  Task UpdateAsync(ShipMethod shipMethod, CancellationToken cancellationToken = default);
  Task DeleteAsync(int shipMethodID, CancellationToken cancellationToken = default);
}