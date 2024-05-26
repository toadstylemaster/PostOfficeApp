using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
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
            var res = (await Repository.AllAsync())
                .Select(x => new App.Public.DTO.v1.BagWithParcels()
                {
                    Id = x.Id,
                    BagNumber = x.BagNumber,
                    ListOfParcels = (ICollection<App.Public.DTO.v1.Parcel>?)x.ListOfParcels
                })
                .ToList();
            return res;
        }

        public async Task<IEnumerable<Public.DTO.v1.BagWithParcels>> GetBagWithParcelsByShipmentId(App.Public.DTO.v1.Shipment shipment)
        {
            var validBagWithParcels = new List<App.Public.DTO.v1.BagWithParcels>();
            
            if(shipment.ListOfBags == null)
            {
                return validBagWithParcels;
            }

            foreach(var item in shipment.ListOfBags)
            {
                var validBag = await Repository.FindAsync(item.Id);
                if (validBag != null)
                {
                    validBagWithParcels.Add(Map(validBag));
                }
            }

            return validBagWithParcels;
        }

        public void PostBagWithParcels(Public.DTO.v1.BagWithParcels bagWithParcels)
        {
            var bagWithParcelsFromDb = new App.DAL.DTO.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                ListOfBags = shipment.ListOfBags,
            };

            if (bagWithParcelsFromDb == null)
            {
                throw new ArgumentNullException("Shipment is invalid!");
            }
            var dalShipment = Repository.Add(bagWithParcelsFromDb);
            var dtoShipment = new App.Public.DTO.v1.Shipment()
            {
                Id = dalShipment.Id,
                ShipmentNumber = dalShipment.ShipmentNumber,
                Airport = dalShipment.Airport,
                FlightNumber = dalShipment.FlightNumber,
                FlightDate = dalShipment.FlightDate,
                ListOfBags = dalShipment.ListOfBags,
            };

            return dtoShipment;
        }

        private IEnumerable<App.Public.DTO.v1.BagWithParcels>? Map(List<DAL.DTO.BagWithParcels>? bags)
        {
            var res = bags.Select(bag =>
            {
                return new App.Public.DTO.v1.BagWithParcels()
                {
                    Id = bag.Id,
                    BagNumber = bag.BagNumber,
                    ListOfParcels = Map(bag.ListOfParcels?.ToList())?.ToList(),
                };
            });
            return res;
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
    }
}
