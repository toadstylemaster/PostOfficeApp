﻿namespace App.DAL.EF
{
    using App.Domain;
    using Microsoft.EntityFrameworkCore;


    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Parcel> Parcels { get; set; } = default!;
        public DbSet<BagWithLetters> BagWithLetters { get; set; } = default!;
        public DbSet<BagWithParcels> BagWithParcels { get; set; } = default!;
        public DbSet<Shipment> Shipments { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bag>()
                .ToTable("Bags");

            builder.Entity<BagWithLetters>()
                .ToTable("BagsWithLetters")
                .HasBaseType<Bag>();

            builder.Entity<BagWithParcels>()
                .ToTable("BagsWithParcels")
                .HasBaseType<Bag>();



            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            builder.Entity<Shipment>()
                .HasMany(s => s.ListOfBags)
                .WithOne(b => b.Shipment)
                .HasForeignKey(b => b.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BagWithParcels>()
                .HasMany(s => s.ListOfParcels)
                .WithOne(b => b.BagWithParcels)
                .HasForeignKey(b => b.BagWithParcelsId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
