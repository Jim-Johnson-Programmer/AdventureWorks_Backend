using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesReasonConfiguration : IEntityTypeConfiguration<SalesReason>
{
  public void Configure(EntityTypeBuilder<SalesReason> builder)
  {
    builder.HasKey(sr => sr.SalesReasonID);
    builder.Property(sr => sr.SalesReasonID).ValueGeneratedOnAdd();
  }
}