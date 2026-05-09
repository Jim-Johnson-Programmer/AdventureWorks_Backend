using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesTerritoryConfiguration : IEntityTypeConfiguration<SalesTerritory>
{
  public void Configure(EntityTypeBuilder<SalesTerritory> builder)
  {
    builder.HasKey(st => st.TerritoryID);
    builder.Property(st => st.TerritoryID).ValueGeneratedOnAdd();
  }
}