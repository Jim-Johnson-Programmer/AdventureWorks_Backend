using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AWMicroservices.SalesOrders.Domain.Interfaces;
using AWMicroservices.SalesOrders.Infrastructure.Persistence;
using AWMicroservices.SalesOrders.Infrastructure.Persistence.Repositories;

namespace AWMicroservices.SalesOrders.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();

    return services;
  }
}