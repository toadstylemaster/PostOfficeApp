using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.EF.Repositories
{
    public class ShipmentRepository : EFBaseRepository<Shipment, App.Domain.Shipment, AppDbContext>, IShipmentRepository
    {
        public ShipmentRepository(AppDbContext dataContext, ShipmentMapper mapper) : base(dataContext, mapper)
        {
        }

        public override async Task<IEnumerable<Shipment>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }

    }
}
