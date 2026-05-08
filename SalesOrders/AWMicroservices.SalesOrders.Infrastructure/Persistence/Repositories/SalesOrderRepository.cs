using Microsoft.EntityFrameworkCore;
using AWMicroservices.SalesOrders.Domain.Entities;
using AWMicroservices.SalesOrders.Domain.Interfaces;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Repositories;

public class SalesOrderRepository(AppDbContext context) : ISalesOrderRepository
{
  public Task<SalesOrder?> GetByIdAsync(int id, CancellationToken ct = default) =>
      context.SalesOrders.FirstOrDefaultAsync(so => so.Id == id, ct);

  public async Task<IEnumerable<SalesOrder>> GetAllAsync(CancellationToken ct = default) =>
      await context.SalesOrders.ToListAsync(ct);

  public async Task AddAsync(SalesOrder salesOrder, CancellationToken ct = default)
  {
    await context.SalesOrders.AddAsync(salesOrder, ct);
    await context.SaveChangesAsync(ct);
  }

  public async Task UpdateAsync(SalesOrder salesOrder, CancellationToken ct = default)
  {
    context.SalesOrders.Update(salesOrder);
    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(int id, CancellationToken ct = default)
  {
    var salesOrder = await GetByIdAsync(id, ct);
    if (salesOrder is not null)
    {
      context.SalesOrders.Remove(salesOrder);
      await context.SaveChangesAsync(ct);
    }
  }
}