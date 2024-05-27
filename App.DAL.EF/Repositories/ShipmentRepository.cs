using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.Contracts.Domain;
using Base.DAL.EF;
using Base.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.EF.Repositories
{
    public class ShipmentRepository : EFBaseRepository<Shipment, App.Domain.Shipment, AppDbContext>, IShipmentRepository
    {
        public ShipmentRepository(AppDbContext dataContext, ShipmentMapper mapper) : base(dataContext, mapper)
        {
        }

        public override async Task<IEnumerable<Shipment>> AllAsync(bool noTracking = true)
        {

            return (await RepositoryDbSet
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }

        public void RemoveBagsFromShipment(ICollection<Bag> bags)
        {
            var bagWithParcels = new BagWithParcels();
            var bagWithLetters = new BagWithLetters();
            var bagsLength = bags.Count();

            for (int i = bagsLength - 1; i >= 0; i--)
            {
                if(bags.ElementAt(i).GetType() == typeof(BagWithParcels))
                {
                    bagWithParcels = (BagWithParcels)bags.ElementAt(i);
                    if (bagWithParcels.ListOfParcels != null && bagWithParcels.ListOfParcels.Any())
                    {
                        var parcelsLength = bagWithParcels.ListOfParcels.Count();
                        for (int j = parcelsLength - 1; j >= 0; j--)
                        {
                            RepositoryDbContext.Parcels.Remove(Map(bagWithParcels.ListOfParcels.ElementAt(j))!);
                        }
                    }
                    RepositoryDbContext.BagWithParcels.Remove(Map(bagWithParcels));
                }
                if (bags.ElementAt(i).GetType() == typeof(BagWithLetters))
                {
                    bagWithLetters = (BagWithLetters)bags.ElementAt(i);
                    RepositoryDbContext.BagWithLetters.Remove(Map(bagWithLetters));
                }
            }
        }

        private App.Domain.BagWithParcels Map(BagWithParcels bag)
        {
            return new App.Domain.BagWithParcels()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                ListOfParcels = Map(bag.ListOfParcels?.ToList())?.ToList(),
            };
        }


        private List<App.Domain.Parcel>? Map(ICollection<DAL.DTO.Parcel>? parcels)
        {
            if (parcels == null)
            {
                return null;
            }
            var res = parcels.Select(parcel =>
            {
                return new App.Domain.Parcel()
                {
                    Id = parcel.Id,
                    ParcelNumber = parcel.ParcelNumber,
                    RecipientName = parcel.RecipientName,
                    DestinationCountry = parcel.DestinationCountry,
                    Price = parcel.Price,
                    Weight = parcel.Weight,
                };
            });
            return res.ToList();
        }
        
        private App.Domain.Parcel? Map(DAL.DTO.Parcel? parcel)
        {
            if (parcel == null)
            {
                return null;
            }
            return new App.Domain.Parcel()
            {
                Id = parcel.Id,
                ParcelNumber = parcel.ParcelNumber,
                RecipientName = parcel.RecipientName,
                DestinationCountry = parcel.DestinationCountry,
                Price = parcel.Price,
                Weight = parcel.Weight,
            };
        }

        private App.Domain.BagWithLetters Map(BagWithLetters bag)
        {
            return new App.Domain.BagWithLetters()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                CountOfLetters = bag.CountOfLetters,
                Price = bag.Price,
                Weight = bag.Weight,
            };
        }


    }
}
