using Microsoft.eShopOnContainers.Services.Stock.API.Infrastructure.EntityConfigurations;

namespace Microsoft.eShopOnContainers.Services.Stock.API.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Model;
    using Microsoft.EntityFrameworkCore.Design;

    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StockEntityTypeConfiguration());
        }     
    }


    public class StockContextDesignFactory : IDesignTimeDbContextFactory<StockContext>
    {
        public StockContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<StockContext>()
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.StockDb;Integrated Security=true");

            return new StockContext(optionsBuilder.Options);
        }
    }
}
