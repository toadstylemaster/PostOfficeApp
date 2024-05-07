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
    }
}
