using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IGenericRepository<E> where E : class
    {
        public Task AddAsync(E item);
        public Task UpdateAsync(E item);
        public Task RemoveAsync(E item);
        public Task<IEnumerable<E>> GetAsync();
        public Task<E> GetByIdAsync(long id);
        public Task AddRangeAsync(IEnumerable<E> item);
        public Task RemoveRangeAsync(IEnumerable<E> items);
        public Task<int> GetCountAsync();
    }
}
