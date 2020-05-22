namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using EntityConfigurations;
    using Model;
    using Microsoft.EntityFrameworkCore.Design;

    public class ShareOwnerContext : DbContext
    {
        public ShareOwnerContext(DbContextOptions<ShareOwnerContext> options) : base(options)
        {
        }
        public DbSet<ShareOwner> ShareOwners { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ShareOwnerEntityTypeConfiguration());
            builder.ApplyConfiguration(new ReservationEntityTypeConfiguration());
        }     
    }


    public class ShareOwnerContextDesignFactory : IDesignTimeDbContextFactory<ShareOwnerContext>
    {
        public ShareOwnerContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<ShareOwnerContext>()
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.ShareOwnerDb;Integrated Security=true");

            return new ShareOwnerContext(optionsBuilder.Options);
        }
    }
}
