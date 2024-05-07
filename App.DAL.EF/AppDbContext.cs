namespace App.DAL.EF
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
        public DbSet<BagWithParcels> BagWithParcels{ get; set; } = default!;
        public DbSet<Shipment> Shipments{ get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Remove cascade delete
            base.OnModelCreating(builder);
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
