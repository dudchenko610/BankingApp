using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<int> AddAsync(TEntity item);
        public Task UpdateAsync(TEntity item);
        public Task RemoveAsync(TEntity item);
        public Task<IList<TEntity>> GetAllAsync();
        public Task<PaginationModel<TEntity>> GetAllAsync(int skip, int take);
        public Task<TEntity> GetByIdAsync(int id);
        public Task AddRangeAsync(IList<TEntity> item);
        public Task RemoveRangeAsync(IList<TEntity> items);
        public Task<int> GetCountAsync();
    }
}
