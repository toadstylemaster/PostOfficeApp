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

        public static void SeedAppData(AppDbContext context)
        {
            SeedAppDataShipments(context, SeedAppDataBagWithParcels(context, SeedAppDataParcels(context).Entity).Entity);
            context.SaveChanges();
        }

        public static EntityEntry<Parcel> SeedAppDataParcels(AppDbContext context)
        {
            return context.Parcels.Add(new Parcel
            {
                ParcelNumber = "ZZ123456YY",
                RecipientName = "Kris Lahe",
                DestinationCountry = "EE",
                Weight = 13,
                Price = 2
            });
        }        
        
        public static EntityEntry<BagWithParcels> SeedAppDataBagWithParcels(AppDbContext context, Parcel parcel)
        {

            return context.BagWithParcels.Add(new BagWithParcels
            {
                BagNumber = "ADBCEFG",
                ListOfParcels = new List<Parcel>{ parcel }
            });
        }

        public static void SeedAppDataShipments(AppDbContext context, BagWithParcels bagWithParcels)
        {
            var listOfParcelBags = new List<Bag>
            {
                bagWithParcels
            };

            context.Shipments.Add(new Shipment
            {
                ShipmentNumber = "AAA-BBBBBB",
                Airport = Airport.HEL,
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
                ListOfBags = listOfParcelBags
            });
        }
    }
}
