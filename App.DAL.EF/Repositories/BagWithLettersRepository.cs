using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories
{
    public class BagWithLettersRepository : EFBaseRepository<BagWithLetters, App.Domain.BagWithLetters, AppDbContext>, IBagWithLettersRepository
    {
        private readonly ShipmentMapper _shipmentMapper;
        private readonly BagMapper _bagMapper;
        public BagWithLettersRepository(AppDbContext dataContext, BagWithLettersMapper mapper, ShipmentMapper shipmentMapper, BagMapper bagMapper) : base(dataContext, mapper)
        {
            _shipmentMapper = shipmentMapper;
            _bagMapper = bagMapper;
        }

        public override async Task<IEnumerable<BagWithLetters>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }

        public async Task<Shipment?> GetShipmentById(Guid? id)
        {
            return _shipmentMapper.Map(await RepositoryDbContext.Shipments.Include(b => b.ListOfBags).AsNoTracking().FirstOrDefaultAsync(s => s.Id == id));
        }

        public override BagWithLetters Add(BagWithLetters bagWithLetters)
        {
            var bag = RepositoryDbContext.BagWithLetters.Any(x => x.BagNumber == bagWithLetters.BagNumber);
            var letterBag = RepositoryDbContext.BagWithParcels.Any(x => x.BagNumber == bagWithLetters.BagNumber);
            if (!bag && !letterBag)
            {
                return base.Add(bagWithLetters);
            }
            throw new ArgumentException("Bag with same bag number already exists!");
        }

        public void ModifyState(BagWithLetters bag)
        {
            RepositoryDbSet.Entry(Mapper.Map(bag)!).State = EntityState.Modified;
        }

        public async Task<Shipment> FindShipment(Guid id)
        {
            return _shipmentMapper.Map(await RepositoryDbContext.Shipments.Include(x => x.ListOfBags).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id))!;
        }

        public async Task<BagWithLetters?> FindByBagNumber(string bagNumber)
        {
            var bag = await RepositoryDbSet.AsNoTracking().FirstOrDefaultAsync(i => i.BagNumber.Equals(bagNumber));

            if (bag is Domain.BagWithLetters bagWithLetters)
            {
                return Mapper.Map(bagWithLetters)!;
            }
            return null;
        }

        public async Task<IEnumerable<Bag>> GetBagWithLettersAsBags()
        {
            var bags = await RepositoryDbSet
                .Select(b => new Bag
                {
                    BagNumber = b.BagNumber,
                    Id = b.Id,
                    ShipmentId = b.ShipmentId
                })
                .ToListAsync();

            return bags.AsEnumerable();
        }

        public void AddShipmentToBagWithLetters(BagWithLetters bag, Shipment shipment)
        {
            bag.ShipmentId = shipment.Id;
            bag.Shipment = shipment;
            ModifyState(bag);
        }
    }
}
