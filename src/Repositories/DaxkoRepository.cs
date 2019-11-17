using DaxkoOrderAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Repositories
{
    public class DaxkoRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DaxkoDbContext _context;
        public IQueryable<TEntity> EntitySet => _context.Set<TEntity>();
        public DaxkoRepository(DaxkoDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public Task<string> GetNewIdAsStringAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entityList)
        {
            await _context.Set<TEntity>().AddRangeAsync(entityList);
            await _context.SaveChangesAsync();
            return entityList;
        }

        public async Task<TEntity> UpdateAsync(int id, TEntity entity)
        {
            TEntity existingEntity = await GetByIdAsync(id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();

                return existingEntity;
            }

            return null;
        }

        public async Task<TEntity> UpdateAsync(string id, TEntity entity)
        {
            TEntity existingEntity = await GetByIdAsync(id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();

                return existingEntity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            TEntity entity = await GetByIdAsync(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
