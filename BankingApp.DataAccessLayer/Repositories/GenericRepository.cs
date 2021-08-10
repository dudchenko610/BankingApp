using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    /// <summary>
    /// Gives common interface to work with data in database.
    /// </summary>
    /// <typeparam name="TEntity">Successor of <see cref="IBaseEntity"/>.</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;

        /// <summary>
        /// Creates instance of <see cref="GenericRepository{TEntity}"/>.
        /// </summary>
        /// <param name="context">An instqance of <see cref="DbContext"/>.</param>
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Adds specified item to the table of databse.
        /// </summary>
        /// <param name="item">Entity to add.</param>
        /// <returns>Id of saved entity.</returns>
        public async Task<int> AddAsync(TEntity item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();

            return item.Id;
        }

        /// <summary>
        /// Removes specified item from database table by id.
        /// </summary>
        /// <param name="item">Entity to remove.</param>
        public async Task RemoveAsync(TEntity item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all elements from table.
        /// </summary>
        /// <returns>List of all entities from table.</returns>
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Updates specified item in database table by id.
        /// </summary>
        /// <param name="item">Entity to update.</param>
        public async Task UpdateAsync(TEntity item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets one entity by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <returns>Entity by specified id.</returns>
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds several entities to database table.
        /// </summary>
        /// <param name="item">List of entities.</param>
        public async Task AddRangeAsync(IList<TEntity> item)
        {
            await _dbSet.AddRangeAsync(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes several entities from database.
        /// </summary>
        /// <param name="item">List of entities.</param>
        public async Task RemoveRangeAsync(IList<TEntity> items)
        {
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get count of elements in database table.
        /// </summary>
        /// <returns>Count of entities.</returns>
        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        /// <summary>
        /// Gets paged elements corresponding to parameters.
        /// </summary>
        /// <param name="skip">Offset from begining of the table.</param>
        /// <param name="take">Number of entities to take.</param>
        /// <returns>View model containing list of paged entities and total table size.</returns>
        public async Task<PaginationModel<TEntity>> GetAllAsync(int skip, int take)
        {
            var items = await _dbSet.Skip(skip).Take(take).ToListAsync();
            var paginationModel = new PaginationModel<TEntity>
            {
                Items = items,
                TotalCount = await _dbSet.CountAsync()
            };

            return paginationModel;
        }
    }
}
