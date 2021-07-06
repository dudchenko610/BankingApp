using BankingApp.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<int> AddAsync(TEntity item);
        public Task UpdateAsync(TEntity item);
        public Task RemoveAsync(TEntity item);
        public Task<IEnumerable<TEntity>> GetAsync();
        public Task<TEntity> GetByIdAsync(int id);
        public Task AddRangeAsync(IEnumerable<TEntity> item);
        public Task RemoveRangeAsync(IEnumerable<TEntity> items);
        public Task<int> GetCountAsync();
    }
}
