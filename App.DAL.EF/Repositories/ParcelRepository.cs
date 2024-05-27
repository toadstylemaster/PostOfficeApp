using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories
{
    public class ParcelRepository : EFBaseRepository<Parcel, App.Domain.Parcel, AppDbContext>, IParcelRepository
    {
        public ParcelRepository(AppDbContext dataContext, ParcelMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<App.Domain.Shipment?> GetShipmentById(Guid? id)
        {
            return await RepositoryDbContext.Shipments.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<IEnumerable<Parcel>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }
    }
}
