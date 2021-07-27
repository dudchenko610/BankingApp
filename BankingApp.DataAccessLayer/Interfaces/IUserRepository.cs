using BankingApp.Entities.Entities;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task BlockUserAsync(int userId, bool block);
    }
}
