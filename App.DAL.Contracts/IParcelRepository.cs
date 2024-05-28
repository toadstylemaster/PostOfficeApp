using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IParcelRepository : IBaseRepository<Parcel>, IParcelRepositoryCustom<Parcel>
    {
        Task<BagWithParcels> FindBagWithParcels(Guid shipmentId);
        void AddBagWithParcelsToParcel(Parcel parcel, BagWithParcels bag);

        void ModifyState(Parcel parcel);
    }

    public interface IParcelRepositoryCustom<TEntity>
    {
    }
}
