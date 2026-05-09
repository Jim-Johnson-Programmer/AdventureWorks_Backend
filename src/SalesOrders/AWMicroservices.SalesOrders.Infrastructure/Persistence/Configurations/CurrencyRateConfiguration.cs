using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.SalesOrders.Domain.Entities;

namespace AWMicroservices.SalesOrders.Infrastructure.Persistence.Configurations;

public class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
{
  public void Configure(EntityTypeBuilder<CurrencyRate> builder)
  {
    builder.HasKey(cr => cr.CurrencyRateID);
    builder.Property(cr => cr.CurrencyRateID).ValueGeneratedOnAdd();
  }
}