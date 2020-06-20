namespace Microsoft.eShopOnContainers.Services.Order.API.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using EntityConfigurations;
    using Model;
    using Microsoft.EntityFrameworkCore.Design;

    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<BuyOrder> BuyOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SalesOrderEntityTypeConfiguration());
            builder.ApplyConfiguration(new BuyOrderEntityTypeConfiguration());
            builder.ApplyConfiguration(new StockEntityTypeConfiguration());
        }     
    }


    public class OrderContextDesignFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<OrderContext>()
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.OrderDb;Integrated Security=true");

            return new OrderContext(optionsBuilder.Options);
        }
    }
}
