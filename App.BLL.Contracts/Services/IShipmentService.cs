using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IShipmentService : IEntityService<Shipment>, IShipmentRepositoryCustom<Shipment>
    {
        Task<IEnumerable<App.Public.DTO.v1.Shipment>> GetShipments();

        Task<App.Public.DTO.v1.Shipment> GetShipment(Guid id);
        Task<bool> PutShipment(Guid id, App.Public.DTO.v1.Shipment shipment);
        Task<bool> PutShipment(Guid id, bool isFinalized);
        void PostShipment(App.Public.DTO.v1.Shipment shipment);
    }
}
