﻿// <auto-generated />
using System;
using App.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.DAL.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240526093619_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("App.Domain.Parcel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BagWithParcelsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DestinationCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParcelNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BagWithParcelsId");

                    b.ToTable("Parcels");
                });

            modelBuilder.Entity("App.Domain.Shipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Airport")
                        .HasColumnType("int");

                    b.Property<DateTime>("FlightDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFinalized")
                        .HasColumnType("bit");

                    b.Property<string>("ShipmentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("Base.Domain.Bag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BagNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<Guid?>("ShipmentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ShipmentId");

                    b.ToTable("Bag");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Bag");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("App.Domain.BagWithLetters", b =>
                {
                    b.HasBaseType("Base.Domain.Bag");

                    b.Property<int>("CountOfLetters")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("ShipmentId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasIndex("ShipmentId1");

                    b.ToTable("Bag", t =>
                        {
                            t.Property("ShipmentId1")
                                .HasColumnName("BagWithLetters_ShipmentId1");
                        });

                    b.HasDiscriminator().HasValue("BagWithLetters");
                });

            modelBuilder.Entity("App.Domain.BagWithParcels", b =>
                {
                    b.HasBaseType("Base.Domain.Bag");

                    b.Property<Guid?>("ShipmentId1")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ShipmentId1");

                    b.HasDiscriminator().HasValue("BagWithParcels");
                });

            modelBuilder.Entity("App.Domain.Parcel", b =>
                {
                    b.HasOne("App.Domain.BagWithParcels", "BagWithParcels")
                        .WithMany("ListOfParcels")
                        .HasForeignKey("BagWithParcelsId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("BagWithParcels");
                });

            modelBuilder.Entity("Base.Domain.Bag", b =>
                {
                    b.HasOne("App.Domain.Shipment", null)
                        .WithMany("ListOfBags")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("App.Domain.BagWithLetters", b =>
                {
                    b.HasOne("App.Domain.Shipment", "Shipment")
                        .WithMany()
                        .HasForeignKey("ShipmentId1")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("App.Domain.BagWithParcels", b =>
                {
                    b.HasOne("App.Domain.Shipment", "Shipment")
                        .WithMany()
                        .HasForeignKey("ShipmentId1")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("App.Domain.Shipment", b =>
                {
                    b.Navigation("ListOfBags");
                });

            modelBuilder.Entity("App.Domain.BagWithParcels", b =>
                {
                    b.Navigation("ListOfParcels");
                });
#pragma warning restore 612, 618
        }
    }
}