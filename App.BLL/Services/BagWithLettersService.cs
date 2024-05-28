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
    public class BagWithLettersService : BaseEntityService<BagWithLetters, DAL.DTO.BagWithLetters, IBagWithLettersRepository>, IBagWithLettersService
    {
        private readonly ShipmentMapper _shipmentMapper;

        public BagWithLettersService(IBagWithLettersRepository repository, BagWithLettersMapper mapper, ShipmentMapper shipmentMapper)
            : base(repository, mapper)
        {
            _shipmentMapper = shipmentMapper;
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


        public async Task<IEnumerable<Bag>> GetBagWithLettersFromListOfBags(List<Bag> bags, Shipment shipment)
        {
            if (bags == null)
            {
                return new List<Bag>();
            }

            foreach (var bag in bags)
            {
                var bagWithLetters = await Repository.FindByBagNumber(bag.BagNumber);
                if (bagWithLetters != null)
                {
                    if (Mapper.Map(bagWithLetters) is BagWithLetters)
                    {
                        var isAdded = await AddShipmentToBagWithLetters(Mapper.Map(bagWithLetters)!, shipment);
                        if (isAdded) { Repository.ModifyState(bagWithLetters); }
                    }
                }
            }
            return bags;
        }

        public async Task<bool> AddShipmentToBagWithLetters(BagWithLetters bag, App.BLL.DTO.Shipment shipment)
        {
            var existingShipment = await Repository.FindShipment(shipment.Id);
            if (existingShipment != null)
            {
                Repository.AddShipmentToBagWithLetters(Mapper.Map(bag)!, existingShipment);
                return true;
            }
            else
            {
                throw new ArgumentException("Shipment with given Id was not found!");
            }
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

        public async Task<bool> RemoveBagsWithLettersFromDB(List<BagWithLetters>? bags)
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
