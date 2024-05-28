using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IShipmentService : IEntityService<Shipment>, IShipmentRepositoryCustom<Shipment>
    {
        Task<IEnumerable<Shipment>> GetShipments();

        Task<Shipment> GetShipment(Guid id);
        Task<bool> PutShipment(Guid id, Shipment shipment);
        Task<bool> PutShipment(Guid id, bool isFinalized);

        void ModifyState(Shipment shipment);
        Shipment PostShipment(Shipment shipment);

        Task<bool> DeleteShipmentFromDb(Guid shipmentId);
        
    }
}
