using BankingApp.Entities.Entities;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    /// <summary>
    ///  Gives interface to work with data in "Users" table.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Blocks / unblocks specified user.
        /// </summary>
        /// <param name="userId">Id of user to block / unblock.</param>
        /// <param name="block">Block operation type (block / unlock).</param>
        /// <returns></returns>
        Task BlockAsync(int userId, bool block);
    }
}
