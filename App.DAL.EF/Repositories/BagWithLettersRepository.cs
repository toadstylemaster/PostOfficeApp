using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories
{
    public class BagWithLettersRepository : EFBaseRepository<BagWithLetters, App.Domain.BagWithLetters, AppDbContext>, IBagWithLettersRepository
    {
        public BagWithLettersRepository(AppDbContext dataContext, BagWithLettersMapper mapper) : base(dataContext, mapper)
        {
        }

        public override async Task<IEnumerable<BagWithLetters>> AllAsync(bool noTracking = true)
        {
            return (await RepositoryDbSet
                .ToListAsync()).Select(x => Mapper.Map(x)!);
        }
    }
}
