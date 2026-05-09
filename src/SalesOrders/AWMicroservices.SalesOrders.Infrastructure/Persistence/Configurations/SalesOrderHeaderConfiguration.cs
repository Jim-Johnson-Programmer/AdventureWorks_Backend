using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesOrderHeaderConfiguration : IEntityTypeConfiguration<SalesOrderHeader>
{
  public void Configure(EntityTypeBuilder<SalesOrderHeader> builder)
  {
    builder.HasKey(soh => soh.SalesOrderID);
    builder.Property(soh => soh.SalesOrderID).ValueGeneratedOnAdd();
    builder.Property(soh => soh.SalesOrderNumber).HasComputedColumnSql("isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID]),N'*** ERROR ***')");
    builder.Property(soh => soh.TotalDue).HasComputedColumnSql("isnull(([SubTotal]+[TaxAmt])+[Freight],(0))");
  }
}