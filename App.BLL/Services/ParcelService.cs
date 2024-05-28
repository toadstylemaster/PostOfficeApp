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
    public class ParcelService : BaseEntityService<Parcel, DAL.DTO.Parcel, IParcelRepository>, IParcelService
    {
        public ParcelService(IParcelRepository repository, ParcelMapper mapper) : base(repository, mapper)
        {
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

        public async Task<IEnumerable<Parcel>> GetParcelsFromBagWithParcels(List<Parcel> parcels, BagWithParcels bagWithParcels)
        {
            if (parcels == null)
            {
                return new List<Parcel>();
            }

            foreach (var parcel in parcels)
            {
                if (parcel != null)
                {
                    var isAdded = await AddBagWithParcelsToParcel(parcel!, bagWithParcels);
                    if (isAdded) { Repository.ModifyState(Mapper.Map(parcel)!); }
                }
            }
            return parcels;
        }

        public async Task<bool> AddBagWithParcelsToParcel(Parcel parcel, BagWithParcels bag)
        {
            var existingBag = await Repository.FindBagWithParcels(bag.Id);
            if (existingBag != null)
            {
                Repository.AddBagWithParcelsToParcel(Mapper.Map(parcel)!, existingBag);
                return true;
            }
            else
            {
                throw new ArgumentException("Shipment with given Id was not found!");
            }
        }

        public Parcel PostParcel(Parcel parcel)
        {
            if (parcel == null)
            {
                throw new ArgumentNullException("Parcel is invalid!");
            }
            return Mapper.Map(Repository.Add(Mapper.Map(parcel)!))!;

        }

        public async Task<bool> RemoveParcelsFromDb(List<Parcel>? parcels)
        {
            if(parcels == null)
            {
                return false;
            }
            var parcelCount = parcels.Count;

            for (int i = parcelCount - 1; i >= 0; i--)
            {
                await Repository.RemoveAsync(parcels.ElementAt(i).Id, true);
            }
            return true;
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
