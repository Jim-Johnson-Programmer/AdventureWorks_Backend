using Microsoft.EntityFrameworkCore;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}