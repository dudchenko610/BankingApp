using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IBaseEntity
    {
        public Task<int> AddAsync(TEntity item);
        public Task UpdateAsync(TEntity item);
        public Task RemoveAsync(TEntity item);
        public Task<IList<TEntity>> GetAllAsync();
        public Task<PagedDataView<TEntity>> GetAllAsync(int skip, int take);
        public Task<TEntity> GetByIdAsync(int id);
        public Task AddRangeAsync(IList<TEntity> item);
        public Task RemoveRangeAsync(IList<TEntity> items);
        public Task<int> GetCountAsync();
    }
}
