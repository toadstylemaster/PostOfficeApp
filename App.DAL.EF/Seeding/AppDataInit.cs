using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using Base.Domain;
using Base.Contracts.Domain;

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

        public async static void SeedAppData(AppDbContext context)
        {
            SeedAppDataShipments(context, (await SeedAppDataBagWithParcels(context, (await SeedAppDataParcels(context)).Entity)).Entity);
            context.SaveChanges();
        }

        public async static Task<EntityEntry<Parcel>> SeedAppDataParcels(AppDbContext context)
        {
            return await context.Parcels.AddAsync(new Parcel
            {
                ParcelNumber = "ZZ123456YY",
                RecipientName = "Kris Lahe",
                DestinationCountry = "EE",
                Weight = 13,
                Price = 2
            });
        }        
        
        public static async Task<EntityEntry<BagWithParcels>> SeedAppDataBagWithParcels(AppDbContext context, Parcel parcel)
        {

            return await context.BagWithParcels.AddAsync(new BagWithParcels
            {
                BagNumber = "ADBCEFG",
                ListOfParcels = new List<Parcel>{ parcel }
            });
        }

        public async static void SeedAppDataShipments(AppDbContext context, BagWithParcels bagWithParcels)
        {
            var listOfParcelBags = new List<Bag>
            {
                bagWithParcels
            };

            await context.Shipments.AddAsync(new Shipment
            {
                ShipmentNumber = "AAA-BBBBBB",
                Airport = "HEL",
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
                ListOfBags = listOfParcelBags
            });
        }
    }
}
