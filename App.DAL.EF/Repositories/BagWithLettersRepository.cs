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

        public async Task<App.Domain.Shipment?> GetShipmentById(Guid? id)
        {
            return await RepositoryDbContext.Shipments.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public override BagWithLetters Add(BagWithLetters bagWithLetters)
        {
            var bag = RepositoryDbContext.BagWithLetters.Any(x => x.BagNumber == bagWithLetters.BagNumber);
            var letterBag = RepositoryDbContext.BagWithParcels.Any(x => x.BagNumber == bagWithLetters.BagNumber);
            if (!bag && !letterBag)
            {
                return base.Add(bagWithLetters);
            }
            throw new ArgumentException("Bag with same bag number already exists!");
        }
    }
}
