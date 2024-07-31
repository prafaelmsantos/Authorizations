namespace Authorizations.Persistence.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity?> FindByIdAsync(long id);

        void Dispose();
    }
}
