using Base.Contracts.DAL;
using Base.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contracts.BLL
{
    public interface IEntityService<TEntity> : IBaseRepository<TEntity>, IEntityService<TEntity, Guid>
        where TEntity : class, IDomainEntityId
    {
    }

    public interface IEntityService<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}
