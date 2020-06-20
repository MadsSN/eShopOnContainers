using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var changedEntities = ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added ||
                            _.State == EntityState.Modified);

            var errors = new List<ValidationResult>(); // all errors are here
            foreach (var e in changedEntities)
            {
                var vc = new ValidationContext(e.Entity, null, null);
                Validator.ValidateObject(
                    e.Entity, vc, true);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }


    public class ShareOwnerContextDesignFactory : IDesignTimeDbContextFactory<ShareOwnerContext>
    {
        public ShareOwnerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShareOwnerContext>()
                .UseSqlServer(
                    "Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.ShareOwnerDb;Integrated Security=true");

            return new ShareOwnerContext(optionsBuilder.Options);
        }
    }
}
