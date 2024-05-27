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

        public async Task<List<App.Public.DTO.v1.Parcel>?> GetParcelsByShipmentId(Guid? shipmentId)
        {
            if (shipmentId == null)
            {
                return new List<App.Public.DTO.v1.Parcel>();
            }
            var validParcels = new List<App.Public.DTO.v1.Parcel>();
            var shipment = await Repository.GetShipmentById(shipmentId);
            var validBag = new App.Domain.BagWithParcels();

            if (shipment == null || shipment.ListOfBags == null)
            {
                return validParcels;
            }

            foreach (var bag in shipment.ListOfBags)
            {
                if (bag.GetType() == typeof(App.Domain.BagWithParcels))
                {
                    validBag = (Domain.BagWithParcels)bag;
                    if (validBag.ListOfParcels != null && validBag.ListOfParcels.Count > 0)
                    {
                        foreach (var parcel in validBag.ListOfParcels)
                        {
                            validParcels.Add(Map(parcel)!);
                        }
                    }
                } 
            }
            

            return validParcels;
        }

        public Public.DTO.v1.Parcel PostParcel(Public.DTO.v1.Parcel parcel)
        {
            var parcelFromDb = new App.DAL.DTO.Parcel()
            {
                Id = parcel.Id,
                ParcelNumber = parcel.ParcelNumber,
                RecipientName = parcel.RecipientName,
                DestinationCountry = parcel.DestinationCountry,
                Price = parcel.Price,
                Weight = parcel.Weight,
            };

            if (parcelFromDb == null)
            {
                throw new ArgumentNullException("Parcel is invalid!");
            }
            var dalParcel = Repository.Add(parcelFromDb);
            var dtoParcel = new App.Public.DTO.v1.Parcel()
            {
                Id = dalParcel.Id,
                ParcelNumber = dalParcel.ParcelNumber,
                RecipientName = dalParcel.RecipientName,
                DestinationCountry = dalParcel.DestinationCountry,
                Price = dalParcel.Price,
                Weight = dalParcel.Weight,
            };

            return dtoParcel;
        }

        public async Task<bool> RemoveParcelsFromDb(List<App.Public.DTO.v1.Parcel>? parcels)
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


        private App.Public.DTO.v1.Parcel? Map(App.Domain.Parcel? parcel)
        {
            if (parcel == null)
            {
                return null;
            }
            return new App.Public.DTO.v1.Parcel()
            {
                Id = parcel.Id,
                ParcelNumber = parcel.ParcelNumber,
                RecipientName = parcel.RecipientName,
                DestinationCountry = parcel.DestinationCountry,
                Price = parcel.Price,
                Weight = parcel.Weight,
            };
        }

    }
}
