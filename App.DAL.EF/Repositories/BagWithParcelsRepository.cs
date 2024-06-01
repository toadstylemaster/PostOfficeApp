using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories
{
    public class BagWithParcelsRepository : EFBaseRepository<BagWithParcels, App.Domain.BagWithParcels, AppDbContext>, IBagWithParcelsRepository
    {
        private readonly ShipmentMapper _shipmentMapper;
        public BagWithParcelsRepository(AppDbContext dataContext, BagWithParcelsMapper mapper, ShipmentMapper shipmentMapper) : base(dataContext, mapper)
        {
            _shipmentMapper = shipmentMapper;
        }

        public override async Task<IEnumerable<BagWithParcels>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet.Include(p => p.ListOfParcels)
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }



        public void ModifyState(BagWithParcels bag)
        {
            RepositoryDbSet.Entry(Mapper.Map(bag)!).State = EntityState.Modified;
        }

        public async Task<BagWithParcels?> FindByBagNumber(string bagNumber)
        {
            var bag = await RepositoryDbSet.Include(p => p.ListOfParcels).AsNoTracking().FirstOrDefaultAsync(i => i.BagNumber.Equals(bagNumber));
            if (bag is Domain.BagWithParcels bagWithParcels)
            {
                return Mapper.Map(bagWithParcels)!;
            }
            return null;

        }

        public async Task<IEnumerable<Bag>> GetBagWithParcelsAsBags()
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

        public void AddShipmentToBagWithParcels(BagWithParcels bag, Shipment shipment)
        {
            bag.ShipmentId = shipment.Id;
            bag.Shipment = shipment;
            ModifyState(bag);

        }

        public override BagWithParcels Add(BagWithParcels bagWithParcels)
        {
            var bag = RepositoryDbContext.BagWithParcels.Any(x => x.BagNumber == bagWithParcels.BagNumber);
            var letterBag = RepositoryDbContext.BagWithLetters.Any(x => x.BagNumber == bagWithParcels.BagNumber);
            if (!bag && !letterBag)
            {
                return base.Add(bagWithParcels);
            }
            throw new ArgumentException("Bag with same bag number already exists!");
        }

        public async Task<Shipment> FindShipment(Guid id)
        {

            return _shipmentMapper.Map(await RepositoryDbContext.Shipments.Include(b => b.ListOfBags).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id))!;

        }
    }
}
