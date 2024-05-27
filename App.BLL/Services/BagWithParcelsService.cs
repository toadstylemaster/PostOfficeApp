using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Services
{
    public class BagWithParcelsService : BaseEntityService<BagWithParcels, DAL.DTO.BagWithParcels, IBagWithParcelsRepository>, IBagWithParcelsService
    {
        public BagWithParcelsService(IBagWithParcelsRepository repository, BagWithParcelsMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<IEnumerable<Public.DTO.v1.BagWithParcels>> GetBagWithParcels()
        {
            var res = (await Repository.AllAsync(true))
                .Select(x => new App.Public.DTO.v1.BagWithParcels()
                {
                    Id = x.Id,
                    BagNumber = x.BagNumber,
                    ListOfParcels = Map(x.ListOfParcels)
                })
                .ToList();
            return res;
        }

        public async Task<IEnumerable<Public.DTO.v1.BagWithParcels>> GetBagWithParcelsByShipmentId(Guid? shipmentId)
        {
            if (shipmentId == null)
            {
                return new List<App.Public.DTO.v1.BagWithParcels>();
            }
            var shipment = await Repository.GetShipmentById(shipmentId);
            var validBagWithParcels = new List<App.Public.DTO.v1.BagWithParcels>();
            
            if(shipment == null || shipment.ListOfBags == null)
            {
                return validBagWithParcels;
            }

            foreach(var item in shipment.ListOfBags)
            {
                var validBag = await Repository.FindAsync(item.Id, true);
                if (validBag != null)
                {
                    validBagWithParcels.Add(Map(validBag));
                }
            }

            return validBagWithParcels;
        }

        public App.Public.DTO.v1.BagWithParcels PostBagWithParcels(Public.DTO.v1.BagWithParcels bagWithParcels)
        {
            var bagWithParcelsFromDb = new App.DAL.DTO.BagWithParcels()
            {
                Id = bagWithParcels.Id,
                BagNumber = bagWithParcels.BagNumber,
                ListOfParcels = Map(bagWithParcels.ListOfParcels)
            };

            if (bagWithParcelsFromDb == null)
            {
                throw new ArgumentNullException("Bag with parcels is invalid!");
            }
            var dalbagWithParcels = Repository.Add(bagWithParcelsFromDb);
            var dtobagWithParcels = new App.Public.DTO.v1.BagWithParcels()
            {
                Id = dalbagWithParcels.Id,
                BagNumber = dalbagWithParcels.BagNumber,
                ListOfParcels = Map(dalbagWithParcels.ListOfParcels)
            };

            return dtobagWithParcels;
        }

        public Public.DTO.v1.BagWithParcels PutParcelsToBag(Guid bagWithParcelsId, List<App.Public.DTO.v1.Parcel> parcels)
        {
            var dalBagWithParcels = Repository.FindAsync(bagWithParcelsId, true).Result;

            if (dalBagWithParcels == null || parcels == null)
            {
                throw new ArgumentException("Error: Either bagWithParcels with given id was not found or parcels were invalid!");
            }
            Repository.Update(dalBagWithParcels);

            var dtoBagWithParcels = new App.Public.DTO.v1.BagWithParcels()
            {
                Id = dalBagWithParcels.Id,
                BagNumber = dalBagWithParcels.BagNumber,
                ListOfParcels = parcels
            };

            return dtoBagWithParcels;
        }

        public async Task<bool> RemoveBagWithParcelsFromDb(Guid id)
        {
            var bagWithParcels = await Repository.FindAsync(id, true);
            if (bagWithParcels == null)
            {
                return false;
            }

            await Repository.RemoveAsync(id, true);
            return true;
        }

        public async Task<bool> RemoveBagsWithParcelsFromDb(List<App.Public.DTO.v1.BagWithParcels>? bags)
        {
            if (bags == null)
            {
                return false;
            }
            var bagCount = bags.Count;

            for (int i = bagCount - 1; i >= 0; i--)
            {
                await Repository.RemoveAsync(bags.ElementAt(i).Id, true);
            }
            return true;
        }

        private App.Public.DTO.v1.BagWithParcels Map(DAL.DTO.BagWithParcels bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new App.Public.DTO.v1.BagWithParcels()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                ListOfParcels = Map(bag.ListOfParcels?.ToList())?.ToList(),
            };
        }

        private List<App.Public.DTO.v1.Parcel>? Map(ICollection<DAL.DTO.Parcel>? parcels)
        {
            if (parcels == null)
            {
                return null;
            }
            var res = parcels.Select(parcel =>
            {
                return new App.Public.DTO.v1.Parcel()
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

        private List<DAL.DTO.Parcel>? Map(ICollection<App.Public.DTO.v1.Parcel>? parcels)
        {
            if (parcels == null)
            {
                return null;
            }
            var res = parcels.Select(parcel =>
            {
                return new DAL.DTO.Parcel()
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

    }
}
