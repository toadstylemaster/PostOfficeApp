using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IBagWithLettersRepository : IBaseRepository<BagWithLetters>, IBagWithLettersRepositoryCustom<BagWithLetters>
    {
    }

    public interface IBagWithLettersRepositoryCustom<TEntity>
    {
    }
}
