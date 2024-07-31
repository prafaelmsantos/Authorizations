namespace Authorizations.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Protected properties
        protected AppDbContext _context;
        protected DbSet<TEntity> _entity;
        #endregion

        #region constructor

        public Repository(AppDbContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();
        }
        #endregion

        #region Public methods

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _entity.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _entity.UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<bool> RemoveAsync(TEntity entity)
        {
            _entity.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual IQueryable<TEntity> GetAll() => _entity;

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }

        public virtual async Task<TEntity?> FindByIdAsync(long id)
        {
            return await _entity.FindAsync(id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null!;
                }
                if (_entity != null)
                {
                    _entity = null!;
                }
            }
        }

        #endregion
    }
}
