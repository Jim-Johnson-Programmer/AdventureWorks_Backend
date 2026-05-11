using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.HasKey(c => c.CustomerID);
    builder.Property(c => c.CustomerID).ValueGeneratedOnAdd();

    builder.Property(c => c.PersonID);
    builder.Property(c => c.StoreID);
    builder.Property(c => c.TerritoryID).IsRequired();
    builder.Property(c => c.AccountNumber)
      .HasComputedColumnSql("isnull('AW'+[dbo].[ufnLeadingZeros]([CustomerID]),'')", stored: true)
      .HasMaxLength(10);
    builder.Property(c => c.Rowguid).IsRequired();
    builder.Property(c => c.ModifiedDate).IsRequired();

    // Foreign keys
    builder.HasOne<Person>()
      .WithMany()
      .HasForeignKey(c => c.PersonID)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne<Store>()
      .WithMany()
      .HasForeignKey(c => c.StoreID)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne<SalesTerritory>()
      .WithMany()
      .HasForeignKey(c => c.TerritoryID)
      .OnDelete(DeleteBehavior.NoAction);

    // Unique constraint on AccountNumber
    builder.HasIndex(c => c.AccountNumber).IsUnique();
  }
}