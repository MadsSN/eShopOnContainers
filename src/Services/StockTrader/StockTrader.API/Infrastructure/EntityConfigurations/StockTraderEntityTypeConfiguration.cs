using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Catalog.API.Model;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.EntityConfigurations
{
    class StockTraderEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.StockTrader>
    {
        public void Configure(EntityTypeBuilder<Model.StockTrader> builder)
        {
            builder.ToTable("StockTraders");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("stocktrader_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired(true)
                .HasMaxLength(50);
        }
    }
}
