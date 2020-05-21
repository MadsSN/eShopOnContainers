using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure.EntityConfigurations
{
    class AccountEntityTypeConfiguration
        : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Fund");

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.Credit)
                .IsRequired();
        }
    }
}
