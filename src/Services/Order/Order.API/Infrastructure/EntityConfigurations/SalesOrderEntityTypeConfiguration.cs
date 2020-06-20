using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Order.API.Model;

namespace Microsoft.eShopOnContainers.Services.Order.API.Infrastructure.EntityConfigurations
{
    class SalesOrderEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.SalesOrder>
    {
        public void Configure(EntityTypeBuilder<Model.SalesOrder> builder)
        {
            builder.ToTable("SalesOrder");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("saleorder_hilo")
                .IsRequired();

            builder.Property(ci => ci.StockTraderId)
                .IsRequired();

            builder.Property(ci => ci.StockId)
                .IsRequired();

            builder.Property(ci => ci.SharesCount)
                .IsRequired();

            builder.Property(ci => ci.Status)
                .IsRequired();
        }
    }
}
