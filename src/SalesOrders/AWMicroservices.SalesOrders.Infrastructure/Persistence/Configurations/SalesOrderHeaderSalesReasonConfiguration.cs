using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesOrderHeaderSalesReasonConfiguration : IEntityTypeConfiguration<SalesOrderHeaderSalesReason>
{
  public void Configure(EntityTypeBuilder<SalesOrderHeaderSalesReason> builder)
  {
    builder.HasKey(soh => new { soh.SalesOrderID, soh.SalesReasonID });
    builder.Property(soh => soh.SalesOrderID).IsRequired();
    builder.Property(soh => soh.SalesReasonID).IsRequired();

    builder.HasOne<SalesOrderHeader>()
      .WithMany()
      .HasForeignKey(soh => soh.SalesOrderID)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne<SalesReason>()
      .WithMany()
      .HasForeignKey(soh => soh.SalesReasonID)
      .OnDelete(DeleteBehavior.Cascade);
  }
}