using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    /// <summary>
    /// Allows to provide operations with users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Allows getting page of users.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <returns>View model with data about all users in storage and users list for specified page.</returns>
        public Task<PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize);
        
        /// <summary>
        /// Allow to block / unblock specified user.
        /// </summary>
        /// <param name="blockUserAdminView">View model containing user id and block operation type (block / unlock).</param>
        public Task<bool> BlockAsync(BlockUserAdminView blockUserAdminView);
    }
}
