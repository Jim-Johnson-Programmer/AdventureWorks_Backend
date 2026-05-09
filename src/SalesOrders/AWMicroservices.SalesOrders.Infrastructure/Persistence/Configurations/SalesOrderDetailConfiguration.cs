using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesOrderDetailConfiguration : IEntityTypeConfiguration<SalesOrderDetail>
{
  public void Configure(EntityTypeBuilder<SalesOrderDetail> builder)
  {
    builder.HasKey(sod => new { sod.SalesOrderID, sod.SalesOrderDetailID });
    builder.Property(sod => sod.SalesOrderDetailID).ValueGeneratedOnAdd();
    builder.Property(sod => sod.LineTotal).HasComputedColumnSql("isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0))");
  }
}