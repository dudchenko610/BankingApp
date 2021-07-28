using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        int GetSignedInUserId();
        Task<PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize);
        Task BlockAsync(BlockUserAdminView blockUserAdminView);
    }
}
