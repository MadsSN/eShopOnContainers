using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure;

namespace Fund.API.Infrastructure.Migrations
{
    [DbContext(typeof(FundContext))]
    partial class FundContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:Sequence:.catalog_brand_hilo", "'catalog_brand_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:Sequence:.catalog_hilo", "'catalog_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:Sequence:.catalog_type_hilo", "'catalog_type_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Fund.API.Model.FundBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "catalog_brand_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("FundBrand");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Fund.API.Model.FundItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "catalog_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<int>("AvailableStock");

                    b.Property<int>("FundBrandId");

                    b.Property<int>("FundTypeId");

                    b.Property<string>("Description");

                    b.Property<int>("MaxStockThreshold");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("OnReorder");

                    b.Property<string>("PictureUri");

                    b.Property<decimal>("Price");

                    b.Property<int>("RestockThreshold");

                    b.HasKey("Id");

                    b.HasIndex("FundBrandId");

                    b.HasIndex("FundTypeId");

                    b.ToTable("Fund");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Fund.API.Model.FundType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "catalog_type_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("FundType");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Fund.API.Model.FundItem", b =>
                {
                    b.HasOne("Microsoft.eShopOnContainers.Services.Fund.API.Model.FundBrand", "FundBrand")
                        .WithMany()
                        .HasForeignKey("FundBrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.eShopOnContainers.Services.Fund.API.Model.FundType", "FundType")
                        .WithMany()
                        .HasForeignKey("FundTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
