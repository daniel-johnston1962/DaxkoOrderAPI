using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> EntitySet { get; }
        Task<int> SaveChangesAsync();
        Task<string> GetNewIdAsStringAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(string id);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entityList);
        Task<TEntity> UpdateAsync(int id, TEntity entity);
        Task<TEntity> UpdateAsync(string id, TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(TEntity entity);
    }
}
