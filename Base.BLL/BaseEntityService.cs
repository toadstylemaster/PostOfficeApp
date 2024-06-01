using Base.Contracts.Base;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.BLL
{
    public class
        BaseEntityService<TBllEntity, TDalEntity, TRepository> :
            BaseEntityService<TBllEntity, TDalEntity, TRepository, Guid>, IEntityService<TBllEntity>
        where TBllEntity : class, IDomainEntityId
        where TDalEntity : class, IDomainEntityId
        where TRepository : IBaseRepository<TDalEntity>
    {
        public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper) : base(repository, mapper)
        {
        }
    }

    public class BaseEntityService<TBllEntity, TDalEntity, TRepository, TKey> : IEntityService<TBllEntity, TKey>
        where TBllEntity : class, IDomainEntityId<TKey>
        where TDalEntity : class, IDomainEntityId<TKey>
        where TRepository : IBaseRepository<TDalEntity, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        protected readonly IMapper<TBllEntity, TDalEntity> Mapper;
        protected readonly TRepository Repository;

        public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<IEnumerable<TBllEntity>> AllAsync(bool noTracking = true)
        {
            return (await Repository.AllAsync(noTracking)).Select(e => Mapper.Map(e)!);
        }

        public async Task<TBllEntity?> FindAsync(TKey id, bool noTracking = true)
        {
            return Mapper.Map(await Repository.FindAsync(id, noTracking));
        }

        public TBllEntity Add(TBllEntity entity)
        {
            return Mapper.Map(Repository.Add(Mapper.Map(entity)!))!;
        }

        public TBllEntity Update(TBllEntity entity)
        {
            var mappedEntity = Mapper.Map(entity)!;
            return Mapper.Map(Repository.Update(Mapper.Map(entity)!))!;
        }

        public TBllEntity Remove(TBllEntity entity)
        {
            return Mapper.Map(Repository.Remove(Mapper.Map(entity)!))!;
        }

        public async Task<TBllEntity?> RemoveAsync(TKey id, bool noTracking = true)
        {
            return Mapper.Map(await Repository.RemoveAsync(id, true));
        }
    }
}
