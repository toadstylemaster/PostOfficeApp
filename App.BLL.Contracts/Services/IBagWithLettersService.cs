using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IBagWithLettersService : IEntityService<BagWithLetters>, IBagWithLettersRepositoryCustom<BagWithLetters>
    {
        Task<IEnumerable<BagWithLetters>> GetBagWithLettersByShipmentId(Guid shipmentId);
        Task<IEnumerable<App.Public.DTO.v1.BagWithLetters>> GetBagWithLetters();

        Task<IEnumerable<Bag>> GetBagWithLettersFromListOfBags(List<Bag> bags, Shipment shipment);

        BagWithLetters PostBagWithLetters(BagWithLetters bagWithLetters);

        Task<bool> RemoveBagWithLettersFromDB(Guid id);
        
        Task<bool> RemoveBagsWithLettersFromDB(List<BagWithLetters>? bags);


    }
}
