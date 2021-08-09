using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Allows to provide operations with users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets user by its email.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns>Requested user.</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Gets id of user that makes request.
        /// </summary>
        /// <returns>Id of user that makes request.</returns>
        int GetSignedInUserId();

        /// <summary>
        /// Allows getting page of users.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <returns>View model with data about all users in storage and users list for specified page.</returns>
        Task<PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Allows to block / unblock specified user.
        /// </summary>
        /// <param name="blockUserAdminView">View model containing user id and block operation type (block / unlock).</param>
        Task BlockAsync(BlockUserAdminView blockUserAdminView);
    }
}
