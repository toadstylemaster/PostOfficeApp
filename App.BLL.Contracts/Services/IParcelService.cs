using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IParcelService : IEntityService<Parcel>, IParcelRepositoryCustom<Parcel>
    {

        Task<IEnumerable<Parcel>> PutParcelsToBagWithParcels(List<Parcel> parcels, BagWithParcels bagWithParcels);

        Task<Parcel> GetParcel(Guid id);

        Parcel PostParcel(Parcel parcel);

        Task<bool> RemoveParcelFromDb(Guid id);

        Task<IEnumerable<Parcel>> GetParcels();
    }
}
