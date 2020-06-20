using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Order.API.Model;

namespace Microsoft.eShopOnContainers.Services.Order.API.Infrastructure.EntityConfigurations
{
    class StockEntityTypeConfiguration
        : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock");

            builder.Property(ci => ci.StockId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(ci => ci.Price)
                .IsRequired();
        }
    }
}
