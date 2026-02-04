using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bulgarikon.Data.Repository
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class
    {
        private readonly BulgarikonDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public Repository(BulgarikonDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await dbSet.AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(id));
        }

        public async Task<TEntity?> GetByIdTrackedAsync(TId id)
        {
            return await dbSet
                .FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(id));
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null) 
                query = query.Where(predicate);
            return await query.CountAsync();
        }

        public IQueryable<TEntity> Query()
            => dbSet.AsNoTracking();

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity?> FirstOrDefaultTrackingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}