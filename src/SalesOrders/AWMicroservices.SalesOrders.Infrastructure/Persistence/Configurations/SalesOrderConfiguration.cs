using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
  public void Configure(EntityTypeBuilder<SalesOrder> builder)
  {
    builder.HasKey(so => so.Id);
    builder.Property(so => so.Id).ValueGeneratedOnAdd();
    builder.Property(so => so.OrderNumber).IsRequired().HasMaxLength(50);
    builder.Property(so => so.OrderDate).IsRequired();
    builder.Property(so => so.TotalAmount).HasPrecision(18, 2);
  }
}