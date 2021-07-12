using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<int> AddAsync(TEntity item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
            return item.Id;
        }

        public async Task RemoveAsync(TEntity item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TEntity>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task UpdateAsync(TEntity item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddRangeAsync(IList<TEntity> item)
        {
            await _dbSet.AddRangeAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IList<TEntity> items)
        {
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
