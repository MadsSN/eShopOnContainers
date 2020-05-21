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
            builder.ToTable("Account");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("account_hilo")
                .IsRequired();

            builder.Property(ci => ci.StockTraderId)
                .IsRequired();


            builder.Property(ci => ci.Credit)
                .IsRequired();
        }
    }
}
