using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure.EntityConfigurations
{
    class FundBrandEntityTypeConfiguration
        : IEntityTypeConfiguration<FundBrand>
    {
        public void Configure(EntityTypeBuilder<FundBrand> builder)
        {
            builder.ToTable("FundBrand");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("catalog_brand_hilo")
               .IsRequired();

            builder.Property(cb => cb.Brand)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
