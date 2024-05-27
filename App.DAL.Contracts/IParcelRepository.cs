using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IParcelRepository : IBaseRepository<Parcel>, IParcelRepositoryCustom<Parcel>
    {
        Task<App.Domain.Shipment?> GetShipmentById(Guid? id);
    }

    public interface IParcelRepositoryCustom<TEntity>
    {
    }
}
