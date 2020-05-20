namespace Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using EntityConfigurations;
    using Model;
    using Microsoft.EntityFrameworkCore.Design;

    public class StockTraderContext : DbContext
    {
        public StockTraderContext(DbContextOptions<StockTraderContext> options) : base(options)
        {
        }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<StockTrader> StockTraders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        //    builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        //    builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
   //         builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new StockTraderEntityTypeConfiguration());
        }     
    }


    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<StockTraderContext>
    {
        public StockTraderContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<StockTraderContext>()
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.StockTraderDb;Integrated Security=true");

            return new StockTraderContext(optionsBuilder.Options);
        }
    }
}
