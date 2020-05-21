namespace Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using EntityConfigurations;
    using Model;
    using Microsoft.EntityFrameworkCore.Design;

    public class FundContext : DbContext
    {
        public FundContext(DbContextOptions<FundContext> options) : base(options)
        {
        }
        public DbSet<FundItem> FundItems { get; set; }
        public DbSet<FundBrand> FundBrands { get; set; }
        public DbSet<FundType> FundTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FundBrandEntityTypeConfiguration());
            builder.ApplyConfiguration(new FundTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new FundItemEntityTypeConfiguration());
        }     
    }


    public class FundContextDesignFactory : IDesignTimeDbContextFactory<FundContext>
    {
        public FundContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<FundContext>()
                .UseSqlServer("Server=.;Initial Fund=Microsoft.eShopOnContainers.Services.FundDb;Integrated Security=true");

            return new FundContext(optionsBuilder.Options);
        }
    }
}
