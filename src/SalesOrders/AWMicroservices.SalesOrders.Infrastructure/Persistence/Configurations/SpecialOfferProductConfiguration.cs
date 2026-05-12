using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class SpecialOfferProductConfiguration : IEntityTypeConfiguration<SpecialOfferProduct>
{
  public void Configure(EntityTypeBuilder<SpecialOfferProduct> builder)
  {
    builder.HasKey(e => new { e.SpecialOfferID, e.ProductID });
    builder.Property(e => e.SpecialOfferID).IsRequired();
    builder.Property(e => e.ProductID).IsRequired();
    builder.Property(e => e.Rowguid).IsRequired();
    builder.HasIndex(e => e.Rowguid).IsUnique();
    builder.Property(e => e.ModifiedDate).IsRequired();

    // No navigation to SpecialOffer entity in domain, so only configure Product FK if navigation exists
    // builder.Property(e => e.SpecialOfferID).IsRequired();
    // builder.Property(e => e.ProductID).IsRequired();
    // builder.Property(e => e.Rowguid).IsRequired();
    // builder.HasIndex(e => e.Rowguid).IsUnique();
    // builder.Property(e => e.ModifiedDate).IsRequired();

    // If Product navigation exists, configure FK
    builder.HasOne<Product>()
      .WithMany()
      .HasForeignKey(e => e.ProductID)
      .OnDelete(DeleteBehavior.NoAction);
  }
}