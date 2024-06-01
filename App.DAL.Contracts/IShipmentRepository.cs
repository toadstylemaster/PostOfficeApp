using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IShipmentRepository : IBaseRepository<Shipment>, IShipmentRepositoryCustom<Shipment>
    {
        void ModifyState(Shipment shipment);


    }

    public interface IShipmentRepositoryCustom<TEntity>
    {
    }



}
