using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IBagWithLettersRepository : IBaseRepository<BagWithLetters>, IBagWithLettersRepositoryCustom<BagWithLetters>
    {
        Task<Shipment?> GetShipmentById(Guid? id);
        void ModifyState(BagWithLetters bag);

        Task<Shipment> FindShipment(Guid shipmentId);

        Task<BagWithLetters?> FindByBagNumber(string bagNumber);

        void AddShipmentToBagWithLetters(BagWithLetters bag, Shipment shipment);

        Task<IEnumerable<Bag>> GetBagWithLettersAsBags();
    }

    public interface IBagWithLettersRepositoryCustom<TEntity>
    {
    }
}
