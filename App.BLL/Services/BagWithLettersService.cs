using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Services
{
    public class BagWithLettersService : BaseEntityService<BagWithLetters, DAL.DTO.BagWithLetters, IBagWithLettersRepository>, IBagWithLettersService
    {
        public BagWithLettersService(IBagWithLettersRepository repository, BagWithLettersMapper mapper) : base(repository, mapper)
        {
        }

        public Task<BagWithLetters?> FindAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BagWithLetters?> RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
