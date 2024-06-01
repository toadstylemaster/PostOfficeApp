using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories
{
    public class ParcelRepository : EFBaseRepository<Parcel, App.Domain.Parcel, AppDbContext>, IParcelRepository
    {
        private readonly BagWithParcelsMapper _bagWithParcelsMapper;
        public ParcelRepository(AppDbContext dataContext, ParcelMapper mapper, BagWithParcelsMapper bagWithParcelsMapper) : base(dataContext, mapper)
        {
            _bagWithParcelsMapper = bagWithParcelsMapper;
        }

        public async Task<BagWithParcels> FindBagWithParcels(Guid id)
        {
            return _bagWithParcelsMapper.Map(await RepositoryDbContext.BagWithParcels.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id))!;
        }
        public override async Task<IEnumerable<Parcel>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet.AsNoTracking()
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }

        public override Parcel Add(Parcel entity)
        {
            var parcel = RepositoryDbSet.Any(x => x.ParcelNumber == entity.ParcelNumber);
            if (!parcel)
            {
                return base.Add(entity);
            }
            throw new ArgumentException("Parcel with same parcel number already exists!");
        }

        public void ModifyState(Parcel parcel)
        {
            RepositoryDbSet.Entry(Mapper.Map(parcel)!).State = EntityState.Modified;
        }

        public void AddBagWithParcelsToParcel(Parcel parcel, BagWithParcels bag)
        {
            parcel.BagWithParcels = bag;
            parcel.BagWithParcelsId = bag.Id;
            ModifyState(parcel);
        }
    }
}
