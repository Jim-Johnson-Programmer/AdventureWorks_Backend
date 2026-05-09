using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
  public void Configure(EntityTypeBuilder<CreditCard> builder)
  {
    builder.HasKey(cc => cc.CreditCardID);
    builder.Property(cc => cc.CreditCardID).ValueGeneratedOnAdd();
  }
}