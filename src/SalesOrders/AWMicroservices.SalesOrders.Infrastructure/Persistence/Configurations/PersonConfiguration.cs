using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
  public void Configure(EntityTypeBuilder<Person> builder)
  {
    builder.HasKey(p => p.BusinessEntityID);
    builder.Property(p => p.BusinessEntityID).ValueGeneratedNever();
    builder.Property(p => p.PersonType).IsRequired().HasMaxLength(2);
    builder.Property(p => p.NameStyle).IsRequired();
    builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
    builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
    builder.Property(p => p.EmailPromotion).IsRequired();
    builder.Property(p => p.Rowguid).IsRequired();
    builder.Property(p => p.ModifiedDate).IsRequired();
    // Optional fields
    builder.Property(p => p.Title).HasMaxLength(8);
    builder.Property(p => p.MiddleName).HasMaxLength(50);
    builder.Property(p => p.Suffix).HasMaxLength(10);
    builder.Property(p => p.AdditionalContactInfo);
    builder.Property(p => p.Demographics);
    // Table mapping (optional, uncomment if needed)
    // builder.ToTable("Person", "Person");
  }
}
