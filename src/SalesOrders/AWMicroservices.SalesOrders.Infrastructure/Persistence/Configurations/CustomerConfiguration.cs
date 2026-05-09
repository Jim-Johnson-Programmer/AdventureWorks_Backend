using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.HasKey(c => c.CustomerID);
    builder.Property(c => c.CustomerID).ValueGeneratedOnAdd();
    builder.Property(c => c.AccountNumber).HasComputedColumnSql("isnull('AW'+[dbo].[ufnLeadingZeros]([CustomerID]),'')");
  }
}