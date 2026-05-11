using Microsoft.EntityFrameworkCore;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Person> Persons => Set<Person>();
  public DbSet<Address> Addresses => Set<Address>();
  public DbSet<Customer> Customers => Set<Customer>();
  public DbSet<Store> Stores => Set<Store>();
  public DbSet<SalesTerritory> SalesTerritories => Set<SalesTerritory>();
  public DbSet<SalesPerson> SalesPersons => Set<SalesPerson>();
  public DbSet<SalesOrderHeader> SalesOrderHeaders => Set<SalesOrderHeader>();
  public DbSet<CreditCard> CreditCards => Set<CreditCard>();
  public DbSet<ShipMethod> ShipMethods => Set<ShipMethod>();
  public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();
  public DbSet<SalesOrderDetail> SalesOrderDetails => Set<SalesOrderDetail>();
  public DbSet<SalesOrderHeaderSalesReason> SalesOrderHeaderSalesReasons => Set<SalesOrderHeaderSalesReason>();
  public DbSet<SpecialOfferProduct> SpecialOfferProducts => Set<SpecialOfferProduct>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<SalesReason> SalesReasons => Set<SalesReason>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}