using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure.EntityConfigurations
{
    class FundTypeEntityTypeConfiguration
        : IEntityTypeConfiguration<FundType>
    {
        public void Configure(EntityTypeBuilder<FundType> builder)
        {
            builder.ToTable("FundType");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("catalog_type_hilo")
               .IsRequired();

            builder.Property(cb => cb.Type)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
