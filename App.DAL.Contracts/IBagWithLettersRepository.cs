using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IBagWithLettersRepository : IBaseRepository<BagWithLetters>, IBagWithLettersRepositoryCustom<BagWithLetters>
    {
        Task<App.Domain.Shipment?> GetShipmentById(Guid? id);
    }

    public interface IBagWithLettersRepositoryCustom<TEntity>
    {
    }
}
