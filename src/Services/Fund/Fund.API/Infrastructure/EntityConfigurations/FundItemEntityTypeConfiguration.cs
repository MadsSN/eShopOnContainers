using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure.EntityConfigurations
{
    class FundItemEntityTypeConfiguration
        : IEntityTypeConfiguration<FundItem>
    {
        public void Configure(EntityTypeBuilder<FundItem> builder)
        {
            builder.ToTable("Fund");

            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(ci => ci.Price)
                .IsRequired(true);

            builder.Property(ci => ci.PictureFileName)
                .IsRequired(false);

            builder.Ignore(ci => ci.PictureUri);

            builder.HasOne(ci => ci.FundBrand)
                .WithMany()
                .HasForeignKey(ci => ci.FundBrandId);

            builder.HasOne(ci => ci.FundType)
                .WithMany()
                .HasForeignKey(ci => ci.FundTypeId);
        }
    }
}
