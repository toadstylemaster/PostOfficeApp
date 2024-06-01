using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services
{
    public class BagWithLettersService : BaseEntityService<BagWithLetters, DAL.DTO.BagWithLetters, IBagWithLettersRepository>, IBagWithLettersService
    {
        private readonly ShipmentMapper _shipmentMapper;
        private readonly BagMapper _bagMapper;

        public BagWithLettersService(IBagWithLettersRepository repository, BagWithLettersMapper mapper, ShipmentMapper shipmentMapper, BagMapper bagMapper)
            : base(repository, mapper)
        {
            _shipmentMapper = shipmentMapper;
            _bagMapper = bagMapper;
        }

        public async Task<bool> RemoveBagWithLettersFromDB(Guid id)
        {
            var bagWithLetters = await Repository.FindAsync(id, true);
            if (bagWithLetters == null)
            {
                return false;
            }

            await Repository.RemoveAsync(bagWithLetters.Id, true);
            return true;
        }


        public async Task<IEnumerable<Bag>> AddBagWithLettersToShipment(List<Bag> bags, Shipment shipment)
        {
            var finalBags = new List<Bag>();
            if (bags == null || shipment == null)
            {
                return finalBags;
            }


            foreach (var bag in bags)
            {
                var bagWithLetters = await Repository.FindByBagNumber(bag.BagNumber);
                if (bagWithLetters != null)
                {
                    if (Mapper.Map(bagWithLetters) is BagWithLetters)
                    {
                        finalBags.Add(Mapper.Map(bagWithLetters)!);
                        AddShipmentToBagWithLetters(Mapper.Map(bagWithLetters)!, shipment);
                    }
                }
            }
            return finalBags;
        }

        public async Task<IEnumerable<Bag>> GetAllBagWithLettersAsBags()
        {
            return (await Repository.GetBagWithLettersAsBags()).Select(p => _bagMapper.Map(p)!);
        }

        public bool AddShipmentToBagWithLetters(BagWithLetters bag, Shipment shipment)
        {
            Repository.AddShipmentToBagWithLetters(Mapper.Map(bag)!, _shipmentMapper.Map(shipment)!);
            return true;
        }



        public async Task<IEnumerable<Public.DTO.v1.BagWithLetters>> GetBagWithLetters()
        {
            var res = (await Repository.AllAsync(true))
                .Select(x => new App.Public.DTO.v1.BagWithLetters()
                {
                    Id = x.Id,
                    BagNumber = x.BagNumber,
                    CountOfLetters = x.CountOfLetters,
                    Price = x.Price,
                    Weight = x.Weight,
                })
                .ToList();
            return res;
        }

        public async Task<IEnumerable<BagWithLetters>> GetBagWithLettersByShipmentId(Guid shipmentId)
        {
            var shipment = await Repository.FindShipment(shipmentId);
            var validBagWithLetters = new List<BagWithLetters>();

            if (shipment == null || shipment.ListOfBags == null)
            {
                return validBagWithLetters;
            }
            var validBag = new BagWithLetters();
            foreach (var item in shipment.ListOfBags)
            {
                validBag = Mapper.Map(await Repository.FindByBagNumber(item.BagNumber));
                if (validBag != null)
                {
                    validBagWithLetters.Add(validBag);
                }
            }

            return validBagWithLetters;
        }

        public BagWithLetters PostBagWithLetters(BagWithLetters bagWithLetters)
        {

            if (bagWithLetters == null)
            {
                throw new ArgumentNullException("Bag with letters is invalid!");
            }
            return Mapper.Map(Repository.Add(Mapper.Map(bagWithLetters)!))!;

        }

    }
}
