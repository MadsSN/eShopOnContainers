using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Model;
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.Infrastructure.EntityConfigurations
{
    class ShareOwnerEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.ShareOwner>
    {
        public void Configure(EntityTypeBuilder<Model.ShareOwner> builder)
        {
            builder.ToTable("ShareOwner");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("shareowner_hilo")
                .IsRequired();

            builder.Property(ci => ci.StockId)
                .IsRequired();

            builder.Property(ci => ci.StockTraderId)
                .IsRequired();

            builder.Property(ci => ci.Shares)
                .IsRequired();

            builder.HasMany(ci => ci.Reservations)
                .WithOne()
                .HasForeignKey(reservation => reservation.ShareOwnerId);
        }
    }

    class ReservationEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.Reservation>
    {
        public void Configure(EntityTypeBuilder<Model.Reservation> builder)
        {
            builder.ToTable("Reservation");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("reservation_hilo")
                .IsRequired();

            builder.Property(ci => ci.Reserved)
                .IsRequired();

            builder.Property(ci => ci.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<ReservationStatus>());
        }
    }
}
