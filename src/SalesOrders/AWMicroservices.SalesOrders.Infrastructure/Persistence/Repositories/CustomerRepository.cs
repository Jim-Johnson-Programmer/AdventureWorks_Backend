using Microsoft.EntityFrameworkCore;
using AWMicroservices.SalesOrders.Domain.Entities;
using AWMicroservices.SalesOrders.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
  private readonly AppDbContext context;
  private readonly ILogger<CustomerRepository> logger;

  public CustomerRepository(AppDbContext context, ILogger<CustomerRepository> logger)
  {
    this.context = context;
    this.logger = logger;
  }

  public async Task<Customer?> GetByIdAsync(int customerID, CancellationToken cancellationToken = default)
      => await context.Customers.FirstOrDefaultAsync(c => c.CustomerID == customerID, cancellationToken);

  public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    this.logger.LogInformation("Retrieving all customers from database");
    return await context.Customers.ToListAsync(cancellationToken);
  }

  public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
  {
    await context.Customers.AddAsync(customer, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);
  }

  public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
  {
    context.Customers.Update(customer);
    await context.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(int customerID, CancellationToken cancellationToken = default)
  {
    var customer = await GetByIdAsync(customerID, cancellationToken);
    if (customer is not null)
    {
      context.Customers.Remove(customer);
      await context.SaveChangesAsync(cancellationToken);
    }
  }
}
