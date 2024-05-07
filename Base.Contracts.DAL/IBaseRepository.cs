namespace Base.Contracts.DAL
{
    using global::Base.Contracts.Domain;


    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
        where TEntity : class, IDomainEntityId
    {
    }

    public interface IBaseRepository<TEntity, in TKey>
        where TEntity : class, IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> AllAsync(bool noTracking = true);

        Task<TEntity?> FindAsync(TKey id, bool noTracking = true);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Remove(TEntity entity);

        Task<TEntity?> RemoveAsync(TKey id, bool noTracking = true);
    }
}
