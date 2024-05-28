using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IParcelService : IEntityService<Parcel>, IParcelRepositoryCustom<Parcel>
    {
        Task<bool> RemoveParcelsFromDb(List<Parcel>? bags);

        Task<List<Parcel>?> GetParcelsByBagWithParcelsId(Guid shipmentId);

        Task<IEnumerable<Parcel>> GetParcelsFromBagWithParcels(List<Parcel> parcels, BagWithParcels bagWithParcels);

        Task<Parcel> GetParcel(Guid id);

        Parcel PostParcel(Parcel parcel);

        Task<bool> RemoveParcelFromDb(Guid id);
    }
}
