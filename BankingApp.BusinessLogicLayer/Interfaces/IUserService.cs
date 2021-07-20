
using BankingApp.Entities.Entities;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<User> GetSignedInUserAsync();
        Task<User> GetUserByEmailAsync(string email);
        int GetSignedInUserId();
    }
}
