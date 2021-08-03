using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpService _httpService;

        public UserService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<bool> BlockAsync(BlockUserAdminView blockUserAdminView)
        {
            return await _httpService.PostAsync($"{Routes.Admin.Route}/{Routes.Admin.BlockUser}", blockUserAdminView);
        }

        public async Task<PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var pagedDataView = await _httpService
               .GetAsync<PagedDataView<UserGetAllAdminViewItem>>($"{Routes.Admin.Route}/{Routes.Admin.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");
            return pagedDataView;
        }
    }
}
