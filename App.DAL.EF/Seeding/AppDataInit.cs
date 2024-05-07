using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;

namespace App.DAL.EF.Seeding
{
    public static class AppDataInit
    {

        public static void MigrateDatabase(AppDbContext context)
        {
            context.Database.Migrate();
        }

        public static void DropDatabase(AppDbContext context)
        {
            context.Database.EnsureDeleted();
        }

        public static void SeedAppData(AppDbContext context)
        {
            SeedAppDataParcels(context);
            context.SaveChanges();
        }

        public static void SeedAppDataParcels(AppDbContext context)
        {
            if (context.Parcels.Any()) return;

            context.Parcels.Add(new Parcel
            {
                ParcelNumber = "ZZ123456YY",
                RecipientName = "Kris Lahe",
                DestinationCountry = "EE",
                Weight = 13,
                Price = 2
            });
        }
    }
}
