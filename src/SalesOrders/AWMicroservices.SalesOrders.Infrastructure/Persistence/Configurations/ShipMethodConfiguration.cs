using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class ShipMethodConfiguration : IEntityTypeConfiguration<ShipMethod>
{
  public void Configure(EntityTypeBuilder<ShipMethod> builder)
  {
    builder.HasKey(sm => sm.ShipMethodID);
    builder.Property(sm => sm.ShipMethodID).ValueGeneratedOnAdd();
  }
}