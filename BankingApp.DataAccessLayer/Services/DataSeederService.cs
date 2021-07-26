using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Entities.Enums;
using BankingApp.Shared.Helpers;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Services
{
    public class DataSeederService : IDataSeederService
    {
        private readonly BankingDbContext _dbContext;
        private readonly AdminCredentialsOptions _adminCredentials;
        private readonly UserManager<User> _userManager;
         
        public DataSeederService(BankingDbContext dbContext, IOptions<AdminCredentialsOptions> adminCredentialsOptions, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _adminCredentials = adminCredentialsOptions.Value;
            _userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            var enumRolesList = EnumHelper.GetValues<RolesEnum>().Select(x => new IdentityRole<int> { Name = x.ToString().ToLower() });
            var rolesInDb = _dbContext.Roles.ToList();

            var rolesToDelete = rolesInDb.Except(enumRolesList);
            var rolesToAdd = enumRolesList.Except(rolesInDb);

            if (rolesToAdd.Any())
            {
                await _dbContext.Roles.AddRangeAsync(rolesToAdd);
            }
            if (rolesToDelete.Any())
            {
                _dbContext.Roles.RemoveRange(rolesToDelete);
            }

            await _dbContext.SaveChangesAsync();

            if (await _userManager.FindByEmailAsync(_adminCredentials.Email) is null)
            {
                var adminUser = new User
                {
                    UserName = _adminCredentials.Nickname,
                    Email = _adminCredentials.Email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(adminUser, _adminCredentials.Password);
                await _userManager.AddToRoleAsync(adminUser, RolesEnum.Admin.ToString().ToLower());
            }
        }
    }
}
