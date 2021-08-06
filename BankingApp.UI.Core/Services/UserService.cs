using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.Services
{
    /// <summary>
    /// Allows to provide operations with users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IHttpService _httpService;

        /// <summary>
        /// Creates instance of <see cref="UserService"/>
        /// </summary>
        /// <param name="httpService">Allows send HTTP request to server.</param>
        public UserService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        /// <summary>
        /// Allow to block / unblock specified user.
        /// </summary>
        /// <param name="blockUserAdminView">View model containing user id and block operation type (block / unlock).</param>
        public async Task<bool> BlockAsync(BlockUserAdminView blockUserAdminView)
        {
            return await _httpService.PostAsync<BlockUserAdminView>($"{Routes.Admin.Route}/{Routes.Admin.BlockUser}", blockUserAdminView);
        }

        /// <summary>
        /// Allows getting page of users.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <returns>View model with data about all users in storage and users list for specified page.</returns>
        public async Task<PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var pagedDataView = await _httpService
               .GetAsync<PagedDataView<UserGetAllAdminViewItem>>($"{Routes.Admin.Route}/{Routes.Admin.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");

            return pagedDataView;
        }
    }
}
