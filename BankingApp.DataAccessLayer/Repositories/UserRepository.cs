using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    /// <summary>
    ///  Gives interface to work with data in "Users" table.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// Creates instance of <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="dbContext">Gives access to database.</param>
        public UserRepository(BankingDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Blocks / unblocks specified user.
        /// </summary>
        /// <param name="userId">Id of user to block / unblock.</param>
        /// <param name="block">Block operation type (block / unlock).</param>
        public async Task BlockAsync(int userId, bool block)
        {
            var user = await _dbSet.FirstOrDefaultAsync(x => x.Id == userId);
            user.IsBlocked = block;

            await _context.SaveChangesAsync();
        }
    }
}
