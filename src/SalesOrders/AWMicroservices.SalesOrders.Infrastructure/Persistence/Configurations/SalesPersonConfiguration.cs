using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SalesPersonConfiguration : IEntityTypeConfiguration<SalesPerson>
{
  public void Configure(EntityTypeBuilder<SalesPerson> builder)
  {
    builder.HasKey(sp => sp.BusinessEntityID);
    builder.Property(sp => sp.BusinessEntityID).IsRequired();
    builder.Property(sp => sp.TerritoryID);
    builder.Property(sp => sp.SalesQuota).HasColumnType("money");
    builder.Property(sp => sp.Bonus).HasColumnType("money").IsRequired();
    builder.Property(sp => sp.CommissionPct).HasColumnType("smallmoney").IsRequired();
    builder.Property(sp => sp.SalesYTD).HasColumnType("money").IsRequired();
    builder.Property(sp => sp.SalesLastYear).HasColumnType("money").IsRequired();
    builder.Property(sp => sp.Rowguid).IsRequired();
    builder.HasIndex(sp => sp.Rowguid).IsUnique();
    builder.Property(sp => sp.ModifiedDate).IsRequired();

    // Foreign keys
    builder.HasOne<Person>()
      .WithOne()
      .HasForeignKey<SalesPerson>(sp => sp.BusinessEntityID)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne<SalesTerritory>()
      .WithMany()
      .HasForeignKey(sp => sp.TerritoryID)
      .OnDelete(DeleteBehavior.NoAction);
  }
}