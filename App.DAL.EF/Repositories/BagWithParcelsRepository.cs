using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.EF.Repositories
{
    public class BagWithParcelsRepository : EFBaseRepository<BagWithParcels, App.Domain.BagWithParcels, AppDbContext>, IBagWithParcelsRepository
    {
        public BagWithParcelsRepository(AppDbContext dataContext, BagWithParcelsMapper mapper) : base(dataContext, mapper)
        {
        }

        public override async Task<IEnumerable<BagWithParcels>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }

        public Task<IEnumerable<BagWithParcels>> GetAllByNameAsync(string partialTitle, bool noTracking = true)
        {
            throw new NotImplementedException();
        }

        public override BagWithParcels Add(BagWithParcels bagWithParcels)
        {
            var bag = RepositoryDbContext.BagWithParcels.Any(x => x.BagNumber == bagWithParcels.BagNumber);
            var letterBag = RepositoryDbContext.BagWithParcels.Any(x => x.BagNumber == bagWithParcels.BagNumber);
            if (!bag && !letterBag)
            {
                return base.Add(bagWithParcels);
            }
            throw new NotImplementedException();
        }
    }
}
