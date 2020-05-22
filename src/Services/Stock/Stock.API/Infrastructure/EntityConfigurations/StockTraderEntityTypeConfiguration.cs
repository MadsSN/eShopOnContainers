using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.eShopOnContainers.Services.Stock.API.Infrastructure
{
    class StockTraderEntityTypeConfiguration
            : IEntityTypeConfiguration<Model.StockTrader>
    {
        public void Configure(EntityTypeBuilder<Model.StockTrader> builder)
        {
            builder.ToTable("StockTrader");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("stock_trader_hilo")
                .IsRequired();

            builder.Property(ci => ci.StockTraderId)
                .IsRequired();
        }
    }
}