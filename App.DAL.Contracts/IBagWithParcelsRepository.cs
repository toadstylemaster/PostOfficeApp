using App.DAL.DTO;
using Base.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Contracts
{
    public interface IBagWithParcelsRepository : IBaseRepository<BagWithParcels>, IBagWithParcelsRepositoryCustom<BagWithParcels>
    {
        Task<IEnumerable<BagWithParcels>> GetAllByNameAsync(string partialTitle, bool noTracking = true);
    }

    public interface IBagWithParcelsRepositoryCustom<TEntity>
    {
    }
}
