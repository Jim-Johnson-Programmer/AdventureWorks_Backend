
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
  public void Configure(EntityTypeBuilder<Store> builder)
  {
    builder.HasKey(s => s.BusinessEntityID);
    builder.Property(s => s.BusinessEntityID).IsRequired();
    builder.Property(s => s.Name).IsRequired();
    builder.Property(s => s.SalesPersonID);
    builder.Property(s => s.Demographics);
    builder.Property(s => s.Rowguid).IsRequired();
    builder.HasIndex(s => s.Rowguid).IsUnique();
    builder.Property(s => s.ModifiedDate).IsRequired();

    // FK: SalesPersonID (nullable)
    builder.HasOne<SalesPerson>()
      .WithMany()
      .HasForeignKey(s => s.SalesPersonID)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
