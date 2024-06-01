using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services
{
    public class BagWithParcelsService : BaseEntityService<BagWithParcels, DAL.DTO.BagWithParcels, IBagWithParcelsRepository>, IBagWithParcelsService
    {
        private readonly ParcelMapper _parcelMapper;
        private readonly ShipmentMapper _shipmentMapper;
        private readonly BagMapper _bagMapper;
        public BagWithParcelsService(IBagWithParcelsRepository repository, BagWithParcelsMapper mapper, ParcelMapper parcelMapper, ShipmentMapper shipmentMapper, BagMapper bagMapper) : base(repository, mapper)
        {
            _parcelMapper = parcelMapper;
            _shipmentMapper = shipmentMapper;
            _bagMapper = bagMapper;
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

            if (shipment == null || shipment.ListOfBags == null)
            {
                return validBagWithParcels;
            }
            var validBag = new BagWithParcels();
            foreach (var item in shipment.ListOfBags)
            {
                validBag = Mapper.Map(await Repository.FindByBagNumber(item.BagNumber));
                if (validBag != null)
                {
                    validBagWithParcels.Add(validBag);
                }
            }

            return validBagWithParcels;
        }

        public async Task<IEnumerable<Bag>> GetAllBagWithParcelsAsBags()
        {
            return (await Repository.GetBagWithParcelsAsBags()).Select(p => _bagMapper.Map(p)!);
        }

        public async Task<IEnumerable<Bag>> AddBagWithParcelsToShipment(List<Bag> bags, Shipment shipment)
        {
            var finalBags = new List<Bag>();
            if (bags == null || shipment == null)
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
                        AddShipmentToBagWithParcels(Mapper.Map(bagWithParcels)!, shipment);
                    }
                }

            }
            return finalBags;
        }

        public bool AddShipmentToBagWithParcels(App.BLL.DTO.BagWithParcels bag, Shipment shipment)
        {
            Repository.AddShipmentToBagWithParcels(Mapper.Map(bag)!, _shipmentMapper.Map(shipment)!);
            return true;
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
    }
}
