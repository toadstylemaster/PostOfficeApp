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
        private readonly ParcelMapper _parcelMapper;
        public BagWithParcelsService(IBagWithParcelsRepository repository, BagWithParcelsMapper mapper, ParcelMapper parcelMapper) : base(repository, mapper)
        {
            _parcelMapper = parcelMapper;
        }

        public async Task<IEnumerable<BagWithParcels>> GetBagWithParcels()
        {
            var res = (await Repository.AllAsync(true))
                .Select(x => new BagWithParcels()
                {
                    Id = x.Id,
                    BagNumber = x.BagNumber,
                    ListOfParcels = x.ListOfParcels?.Select(parcel => _parcelMapper.Map(parcel)!).ToList(),
                })
                .ToList();
            return res;
        }

        public async Task<IEnumerable<BagWithParcels>> GetBagWithParcelsByShipmentId(Guid shipmentId)
        {
            var shipment = await Repository.FindShipment(shipmentId);
            if (shipment == null)
            {
                return new List<BagWithParcels>();
            }
            var validBagWithParcels = new List<BagWithParcels>();
            
            if(shipment == null || shipment.ListOfBags == null)
            {
                return validBagWithParcels;
            }
            var validBag = new BagWithParcels();
            foreach(var item in shipment.ListOfBags)
            {
                validBag = Mapper.Map(await Repository.FindByBagNumber(item.BagNumber));
                if (validBag != null)
                {
                    validBagWithParcels.Add(validBag);
                }
            }

            return validBagWithParcels;
        }

        public async Task<IEnumerable<Bag>> GetBagWithParcelsFromListOfBags(List<Bag> bags, Shipment shipment)
        {
            var finalBags = new List<Bag>();
            if (bags == null)
            {
                return finalBags.AsEnumerable();
            }

            foreach (var bag in bags)
            {
                var bagWithParcels = await Repository.FindByBagNumber(bag.BagNumber);
                if (bagWithParcels != null)
                {
                    if (Mapper.Map(bagWithParcels) is BagWithParcels)
                    {
                        finalBags.Add(Mapper.Map(bagWithParcels)!);
                        await AddShipmentToBagWithParcels(Mapper.Map(bagWithParcels)!, shipment);
                        Repository.ModifyState(bagWithParcels);
                    }
                }

            }
            return finalBags;
        }

        public void ModifyState(BagWithParcels bagWithParcels)
        {
            Repository.ModifyState(Mapper.Map(bagWithParcels)!);
        }

        public async Task<bool> AddShipmentToBagWithParcels(App.BLL.DTO.BagWithParcels bag, App.BLL.DTO.Shipment shipment)
        {
            var existingShipment = await Repository.FindShipment(shipment.Id);
            if (existingShipment != null)
            {
                Repository.AddShipmentToBagWithParcels(Mapper.Map(bag)!, existingShipment);
                return true;
            }
            else
            {
                throw new ArgumentException("Shipment with given Id was not found!");
            }
        }

        public BagWithParcels PostBagWithParcels(BagWithParcels bagWithParcels)
        {
            var bagWithParcelsFromDb = new App.DAL.DTO.BagWithParcels()
            {
                Id = bagWithParcels.Id,
                BagNumber = bagWithParcels.BagNumber,
                ListOfParcels = bagWithParcels.ListOfParcels?.Select(x => _parcelMapper.Map(x)!).ToList()
            };

            if (bagWithParcelsFromDb == null)
            {
                throw new ArgumentNullException("Bag with parcels is invalid!");
            }
            return Mapper.Map(Repository.Add(bagWithParcelsFromDb))!;

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

        public async Task<bool> RemoveBagsWithParcelsFromDb(List<BagWithParcels>? bags)
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

    }
}
