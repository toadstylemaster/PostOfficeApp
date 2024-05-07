using Base.Contracts.Base;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Base.DAL.EF
{
    public class EFBaseRepository<TDalEntity, TDomainEntity, TDbContext> : EFBaseRepository<TDalEntity, TDomainEntity, Guid, TDbContext>,
        IBaseRepository<TDalEntity>
        where TDalEntity : class, IDomainEntityId
        where TDomainEntity : class, IDomainEntityId
        where TDbContext : DbContext
    {
        public EFBaseRepository(TDbContext dataContext, IMapper<TDalEntity, TDomainEntity> mapper) : base(dataContext, mapper)
        {
        }
    }

    public class EFBaseRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IBaseRepository<TDalEntity, TKey>
        where TDalEntity : class, IDomainEntityId<TKey>
        where TDomainEntity : class, IDomainEntityId<TKey>
        where TKey : struct, IEquatable<TKey>
        where TDbContext : DbContext
    {
        protected TDbContext RepositoryDbContext;
        protected DbSet<TDomainEntity> RepositoryDbSet;
        protected IMapper<TDalEntity, TDomainEntity> Mapper;

        public EFBaseRepository(TDbContext dataContext, IMapper<TDalEntity, TDomainEntity> mapper)
        {
            RepositoryDbContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            RepositoryDbSet = RepositoryDbContext.Set<TDomainEntity>();
            Mapper = mapper;
        }

        protected virtual IQueryable<TDomainEntity> CreateQuery(bool noTracking = true)
        {
            // TODO: entity ownership control

            var query = RepositoryDbSet.AsQueryable();
            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public virtual async Task<IEnumerable<TDalEntity>> AllAsync(bool noTracking = true)
        {
            return (await CreateQuery(noTracking).ToListAsync()).Select(x => Mapper.Map(x)!);
        }

        public virtual async Task<TDalEntity?> FindAsync(TKey id, bool noTracking = true)
        {
            return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(a => a.Id.Equals(id)));
        }

        public virtual TDalEntity Add(TDalEntity entity)
        {
            return Mapper.Map(RepositoryDbSet.Add(Mapper.Map(entity)!).Entity)!;
        }


        public virtual TDalEntity Update(TDalEntity entity)
        {
            return Mapper.Map(RepositoryDbSet.Update(Mapper.Map(entity)!).Entity)!;
        }

        public virtual TDalEntity Remove(TDalEntity entity)
        {
            return Mapper.Map(RepositoryDbSet.Remove(Mapper.Map(entity)!).Entity)!;
        }

        public virtual async Task<TDalEntity?> RemoveAsync(TKey id, bool noTracking = true)
        {
            var entity = await CreateQuery(noTracking).FirstOrDefaultAsync(a => a.Id.Equals(id));

            return entity != null ? Remove(Mapper.Map(entity)!) : null;
        }

        public virtual IEnumerable<TDalEntity> All()
        {
            var x = RepositoryDbSet.ToList();
            var list = new List<TDalEntity>();
            foreach (var entity in x)
            {
                list.Add(Mapper.Map(entity)!);
            }

            return list;
        }

        public virtual TDalEntity? Find(TKey id)
        {
            return Mapper.Map(RepositoryDbSet.Find(id));
        }
    }
}
