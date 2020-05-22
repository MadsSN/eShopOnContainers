using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Stock.API.Model;

namespace Microsoft.eShopOnContainers.Services.Stock.API.Infrastructure.EntityConfigurations
{
    class StockEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.Stock>
    {
        public void Configure(EntityTypeBuilder<Model.Stock> builder)
        {
            builder.ToTable("Stock");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("stock_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();


            builder.Property(ci => ci.Price)
                .IsRequired();
        }
    }
}
