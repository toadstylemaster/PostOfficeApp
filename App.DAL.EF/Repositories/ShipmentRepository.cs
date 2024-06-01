using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories
{
    public class ShipmentRepository : EFBaseRepository<Shipment, App.Domain.Shipment, AppDbContext>, IShipmentRepository
    {
        public ShipmentRepository(AppDbContext dataContext, ShipmentMapper mapper) : base(dataContext, mapper)
        {
        }

        public override async Task<IEnumerable<Shipment>> AllAsync(bool noTracking = true)
        {

            return (await RepositoryDbSet.Include(b => b.ListOfBags).AsNoTracking()
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }

        public override Shipment Add(Shipment entity)
        {
            var shipment = RepositoryDbSet.Any(x => x.ShipmentNumber == entity.ShipmentNumber);
            if (!shipment)
            {
                return base.Add(entity);
            }
            throw new ArgumentException("Shipment with same shipment number already exists!");
        }

        public override async Task<Shipment?> FindAsync(Guid id, bool noTracking = true)
        {
            return Mapper.Map(await RepositoryDbSet.Include(b => b.ListOfBags).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id));
        }

        public void ModifyState(Shipment shipment)
        {
            RepositoryDbSet.Entry(Mapper.Map(shipment)!).State = EntityState.Modified;
        }

    }
}
