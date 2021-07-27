using BankingApp.Entities.Entities;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task BlockAsync(int userId, bool block);
    }
}
