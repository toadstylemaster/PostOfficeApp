using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;
using Base.Domain;

namespace App.BLL.Contracts.Services
{
    public interface IShipmentService : IEntityService<Shipment>, IShipmentRepositoryCustom<Shipment>
    {
        Task<IEnumerable<App.Public.DTO.v1.Shipment>> GetShipments();

        Task<App.Public.DTO.v1.Shipment> GetShipment(Guid id);
        Task<bool> PutShipment(Guid id, App.Public.DTO.v1.Shipment shipment);
        App.Public.DTO.v1.Shipment PutBagsToShipment(Guid shipmentId, List<App.Public.DTO.v1.Bag> bags);
        Task<bool> PutShipment(Guid id, bool isFinalized);
        App.Public.DTO.v1.Shipment PostShipment(App.Public.DTO.v1.Shipment shipment);

        Task<bool> DeleteShipmentFromDb(Guid shipmentId);
        
    }
}
