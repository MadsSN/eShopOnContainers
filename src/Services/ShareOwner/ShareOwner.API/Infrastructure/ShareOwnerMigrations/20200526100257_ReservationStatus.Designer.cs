﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Infrastructure;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    [DbContext(typeof(ShareOwnerContext))]
    [Migration("20200526100257_ReservationStatus")]
    partial class ReservationStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.reservation_hilo", "'reservation_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.shareowner_hilo", "'shareowner_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.ShareOwner.API.Model.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "reservation_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<decimal>("Reserved")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SalesOrderId")
                        .HasColumnType("int");

                    b.Property<int>("ShareOwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShareOwnerId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.ShareOwner.API.Model.ShareOwner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "shareowner_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<decimal>("Shares")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<int>("StockTraderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShareOwner");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.ShareOwner.API.Model.Reservation", b =>
                {
                    b.HasOne("Microsoft.eShopOnContainers.Services.ShareOwner.API.Model.ShareOwner", null)
                        .WithMany("Reservations")
                        .HasForeignKey("ShareOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
