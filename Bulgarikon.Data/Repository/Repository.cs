using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
                return null;

            context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public Task UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteByIdAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}