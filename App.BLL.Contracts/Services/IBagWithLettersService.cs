using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Contracts.Services
{
    public interface IBagWithLettersService : IEntityService<BagWithLetters>, IBagWithLettersRepositoryCustom<BagWithLetters>
    {
        Task<IEnumerable<App.Public.DTO.v1.BagWithLetters>> GetBagWithLettersByShipmentId(Guid shipmentId);
        Task<IEnumerable<App.Public.DTO.v1.BagWithLetters>> GetBagWithLetters();

        App.Public.DTO.v1.BagWithLetters PostBagWithLetters(App.Public.DTO.v1.BagWithLetters bagWithLetters);

        Task<bool> RemoveBagWithLettersFromDB(Guid id);
        
        Task<bool> RemoveBagsWithLettersFromDB(List<App.Public.DTO.v1.BagWithLetters>? bags);


    }
}
