using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IUserService
    {
        public Task<PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize);
        public Task<bool> BlockAsync(BlockUserAdminView blockUserAdminView);
    }
}
