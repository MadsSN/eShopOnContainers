using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Order.API.Model;

namespace Microsoft.eShopOnContainers.Services.Order.API.Infrastructure.EntityConfigurations
{
    class BuyOrderEntityTypeConfiguration
        : IEntityTypeConfiguration<BuyOrder>
    {
        public void Configure(EntityTypeBuilder<BuyOrder> builder)
        {
            builder.ToTable("BuyOrder");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("buyorder_hilo")
                .IsRequired();

            builder.Property(ci => ci.StockTraderId)
                .IsRequired();

            builder.Property(ci => ci.StockId)
                .IsRequired();

            builder.Property(ci => ci.SharesCount)
                .IsRequired();

            builder.Property(ci => ci.PricePerShare)
                .IsRequired();

            builder.Property(ci => ci.Status)
                .IsRequired();
        }
    }
}
