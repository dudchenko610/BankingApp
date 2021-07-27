using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BankingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task BlockUserAsync(int userId, bool block)
        {
            var user = await _dbSet.FirstOrDefaultAsync(x => x.Id == userId);
            user.IsBlocked = block;
            await _context.SaveChangesAsync();
        }
    }
}
