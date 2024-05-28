using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            RepositoryDbSet.Entry(Mapper.Map(bag)!).State = EntityState.Detached;
        }

        public async Task<BagWithParcels> FindByBagNumber(string bagNumber)
        {
            return Mapper.Map(await RepositoryDbSet.AsNoTracking().FirstOrDefaultAsync(i => i.BagNumber.Equals(bagNumber)))!;
        }

        public void AddShipmentToBagWithParcels(BagWithParcels bag, Shipment shipment)
        {
            bag.Shipment = shipment;
            bag.ShipmentId = shipment.Id;
            RepositoryDbSet.Update(Mapper.Map(bag)!);
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
            
           return _shipmentMapper.Map(await RepositoryDbContext.Shipments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id))!;
            
        }
    }
}
