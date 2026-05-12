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

  public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
  {
    logger.LogDebug($"Retrieving customers page {pageNumber} with size {pageSize}");
    return await context.Customers
      .OrderBy(c => c.CustomerID)
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync(cancellationToken);
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

  public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = null, CancellationToken cancellationToken = default)
  {
    logger.LogInformation($"Retrieving customers page {pageNumber} with size {pageSize}, sortBy: {sortBy}, sortDirection: {sortDirection}");
    var query = context.Customers.AsQueryable();

    // Sorting
    if (!string.IsNullOrEmpty(sortBy))
    {
      // Only allow known columns for sorting
      switch (sortBy.ToLower())
      {
        case "customerid":
          query = sortDirection == "desc" ? query.OrderByDescending(c => c.CustomerID) : query.OrderBy(c => c.CustomerID);
          break;
        case "accountnumber":
          query = sortDirection == "desc" ? query.OrderByDescending(c => c.AccountNumber) : query.OrderBy(c => c.AccountNumber);
          break;
        case "modifieddate":
          query = sortDirection == "desc" ? query.OrderByDescending(c => c.ModifiedDate) : query.OrderBy(c => c.ModifiedDate);
          break;
        default:
          query = query.OrderBy(c => c.CustomerID);
          break;
      }
    }
    else
    {
      query = query.OrderBy(c => c.CustomerID);
    }

    var totalCount = await query.CountAsync(cancellationToken);
    var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    return (items, totalCount);
  }
}
