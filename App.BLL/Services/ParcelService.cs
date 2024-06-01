using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services
{
    public class ParcelService : BaseEntityService<Parcel, DAL.DTO.Parcel, IParcelRepository>, IParcelService
    {
        private readonly BagWithParcelsMapper _bagWithParcelsMapper;
        public ParcelService(IParcelRepository repository, ParcelMapper mapper, BagWithParcelsMapper bagWithParcelsMapper) : base(repository, mapper)
        {
            _bagWithParcelsMapper = bagWithParcelsMapper;
        }

        public async Task<List<Parcel>?> GetParcelsByBagWithParcelsId(Guid bagWithParcelsId)
        {
            var bagWithParcels = await Repository.FindBagWithParcels(bagWithParcelsId);
            if (bagWithParcels == null)
            {
                return new List<Parcel>();
            }
            var validParcels = new List<Parcel>();
            var validBag = new BagWithParcels();

            if (bagWithParcels == null || bagWithParcels.ListOfParcels == null)
            {
                return validParcels;
            }



            return validParcels;
        }

        public async Task<Parcel> GetParcel(Guid id)
        {
            var parcel = await Repository.FindAsync(id, true);
            if (parcel == null)
            {
                throw new Exception("Cant find shipment with given id.");
            }

            return Mapper.Map(parcel)!;
        }

        public IEnumerable<Parcel> PutParcelsToBagWithParcels(List<Parcel> parcels, BagWithParcels bagWithParcels)
        {
            if (parcels == null)
            {
                return new List<Parcel>();
            }

            foreach (var parcel in parcels)
            {
                AddBagWithParcelsToParcel(parcel!, bagWithParcels);

            }
            return parcels;
        }

        public bool AddBagWithParcelsToParcel(Parcel parcel, BagWithParcels bag)
        {
            Repository.AddBagWithParcelsToParcel(Mapper.Map(parcel)!, _bagWithParcelsMapper.Map(bag)!);
            return true;
        }

        public Parcel PostParcel(Parcel parcel)
        {
            if (parcel == null)
            {
                throw new ArgumentNullException("Parcel is invalid!");
            }
            return Mapper.Map(Repository.Add(Mapper.Map(parcel)!))!;

        }

        public async Task<IEnumerable<Parcel>> GetParcels()
        {
            var res = (await Repository.AllAsync(true))
                .Select(x => new Parcel()
                {
                    Id = x.Id,
                    ParcelNumber = x.ParcelNumber,
                    RecipientName = x.RecipientName,
                    DestinationCountry = x.DestinationCountry,
                    Price = x.Price,
                    Weight = x.Weight,
                    BagWithParcelsId = x.BagWithParcelsId,
                    BagWithParcels = _bagWithParcelsMapper.Map(x.BagWithParcels)
                })
                .ToList();
            return res;
        }

        public async Task<bool> RemoveParcelFromDb(Guid id)
        {
            var parcel = await Repository.FindAsync(id, true);
            if (parcel == null)
            {
                return false;
            }

            await Repository.RemoveAsync(id, true);
            return true;
        }

    }
}
