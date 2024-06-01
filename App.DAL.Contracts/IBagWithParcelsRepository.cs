using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IBagWithParcelsRepository : IBaseRepository<BagWithParcels>, IBagWithParcelsRepositoryCustom<BagWithParcels>
    {
        void ModifyState(BagWithParcels bag);

        Task<BagWithParcels?> FindByBagNumber(string bagNumber);

        Task<Shipment> FindShipment(Guid shipmentId);

        void AddShipmentToBagWithParcels(BagWithParcels bag, Shipment shipment);

        Task<IEnumerable<Bag>> GetBagWithParcelsAsBags();
    }

    public interface IBagWithParcelsRepositoryCustom<TEntity>
    {
    }
}
