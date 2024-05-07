using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IParcelRepository : IBaseRepository<Parcel>, IParcelRepositoryCustom<Parcel>
    {
    }

    public interface IParcelRepositoryCustom<TEntity>
    {
    }
}
