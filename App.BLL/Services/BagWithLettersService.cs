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
        public BagWithLettersService(IBagWithLettersRepository repository, BagWithLettersMapper mapper) : base(repository, mapper)
        {
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

        public async Task<IEnumerable<Public.DTO.v1.BagWithLetters>> GetBagWithLettersByShipmentId(Guid shipmentId)
        {
            var shipment = await Repository.GetShipmentById(shipmentId);
            var validBagWithLetters = new List<App.Public.DTO.v1.BagWithLetters>();

            if (shipment == null || shipment.ListOfBags == null)
            {
                return validBagWithLetters;
            }

            foreach (var item in shipment.ListOfBags)
            {
                var validBag = await Repository.FindAsync(item.Id, true);
                if (validBag != null)
                {
                    validBagWithLetters.Add(Map(validBag));
                }
            }

            return validBagWithLetters;
        }

        public Public.DTO.v1.BagWithLetters PostBagWithLetters(Public.DTO.v1.BagWithLetters bagWithLetters)
        {
            var bagWithLettersFromDb = new App.DAL.DTO.BagWithLetters()
            {
                Id = bagWithLetters.Id,
                BagNumber = bagWithLetters.BagNumber,
                CountOfLetters = bagWithLetters.CountOfLetters,
                Price = bagWithLetters.Price,
                Weight = bagWithLetters.Weight,
            };

            if (bagWithLettersFromDb == null)
            {
                throw new ArgumentNullException("Bag with parcels is invalid!");
            }
            var dalBagWithLetters = Repository.Add(bagWithLettersFromDb);
            var dtoBagWithLetters = new App.Public.DTO.v1.BagWithLetters()
            {
                Id = dalBagWithLetters.Id,
                BagNumber = dalBagWithLetters.BagNumber,
                CountOfLetters = dalBagWithLetters.CountOfLetters,
                Price = dalBagWithLetters.Price,
                Weight = dalBagWithLetters.Weight,
            };

            return dtoBagWithLetters;
        }

        private App.Public.DTO.v1.BagWithLetters Map(DAL.DTO.BagWithLetters bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new App.Public.DTO.v1.BagWithLetters()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                CountOfLetters = bag.CountOfLetters,
                Price = bag.Price,
                Weight = bag.Weight,
            };
        }

        public async Task<bool> RemoveBagsWithLettersFromDB(List<App.Public.DTO.v1.BagWithLetters>? bags)
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
