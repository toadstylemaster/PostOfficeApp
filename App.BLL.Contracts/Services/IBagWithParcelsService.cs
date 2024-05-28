using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IBagWithParcelsService : IEntityService<BagWithParcels>, IBagWithParcelsRepositoryCustom<BagWithParcels>
    {
        Task<IEnumerable<BagWithParcels>> GetBagWithParcelsByShipmentId(Guid shipmentId);

        Task<IEnumerable<Bag>> GetBagWithParcelsFromListOfBags(List<Bag> bags, Shipment shipment);
        Task<IEnumerable<BagWithParcels>> GetBagWithParcels();

        BagWithParcels PostBagWithParcels(BagWithParcels bagWithParcels);

        Task<bool> RemoveBagWithParcelsFromDb(Guid id);

        Task<bool> RemoveBagsWithParcelsFromDb(List<BagWithParcels>? bags);

        void ModifyState(BagWithParcels bagWithParcels);
    }
}
