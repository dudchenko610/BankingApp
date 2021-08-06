using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Gives common interface to work with data in storage.
    /// </summary>
    /// <typeparam name="TEntity">Successor of <see cref="IBaseEntity"/>.</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class, IBaseEntity
    {
        /// <summary>
        /// Adds specified item to storage.
        /// </summary>
        /// <param name="item">Entity to add.</param>
        /// <returns>Id of saved entity.</returns>
        public Task<int> AddAsync(TEntity item);

        /// <summary>
        /// Updates specified item in storage by id.
        /// </summary>
        /// <param name="item">Entity to update.</param>
        public Task UpdateAsync(TEntity item);

        /// <summary>
        /// Removes specified item from storage by id.
        /// </summary>
        /// <param name="item">Entity to remove.</param>
        public Task RemoveAsync(TEntity item);

        /// <summary>
        /// Gets all elements from table.
        /// </summary>
        /// <returns>List of all entities from table.</returns>
        public Task<IList<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets paged elements corresponding to parameters.
        /// </summary>
        /// <param name="skip">Offset from begining of the table.</param>
        /// <param name="take">Number of entities to take.</param>
        /// <returns>View model containing list of paged entities and total table size.</returns>
        public Task<PaginationModel<TEntity>> GetAllAsync(int skip, int take);

        /// <summary>
        /// Gets one entity by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <returns>Entity by specified id.</returns>
        public Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Adds several entities to storage.
        /// </summary>
        /// <param name="item">List of entities.</param>
        public Task AddRangeAsync(IList<TEntity> item);

        /// <summary>
        /// Removes several entities from storage.
        /// </summary>
        /// <param name="item">List of entities.</param>
        public Task RemoveRangeAsync(IList<TEntity> items);

        /// <summary>
        /// Get count of elements in the table.
        /// </summary>
        /// <returns>Count of entities.</returns>
        public Task<int> GetCountAsync();
    }
}
