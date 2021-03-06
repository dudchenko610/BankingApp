using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Entities.Enums;
using BankingApp.Shared;
using BankingApp.Shared.Helpers;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using static BankingApp.Shared.Extensions.LinqExtensions;

namespace BankingApp.DataAccessLayer.Services
{
    /// <summary>
    /// Initializes database with initial data.
    /// </summary>
    public class DataSeederService : IDataSeederService
    {
        private readonly BankingDbContext _dbContext;
        private readonly AdminCredentialsOptions _adminCredentials;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        /// <summary>
        /// Creates instance of <see cref="DataSeederService"/>.
        /// </summary>
        /// <param name="dbContext">An instance of <see cref="BankingDbContext"/>.</param>
        /// <param name="adminCredentialsOptions">Contains view model with admin credential options mapped from appsettings.</param>
        /// <param name="userManager">An instance of <see cref="UserManager{User}"/>.</param>
        /// <param name="roleManager">An instance of <see cref="RoleManager{IdentityRole{int}}"/>.</param>
        public DataSeederService(BankingDbContext dbContext, IOptions<AdminCredentialsOptions> adminCredentialsOptions, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _dbContext = dbContext;
            _adminCredentials = adminCredentialsOptions.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Seeds roles and admin user into corresponding database tables.
        /// </summary>
        public async Task SeedDataAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
        }

        private async Task SeedRolesAsync()
        {
            var enumRolesList = EnumHelper.GetValues<RolesEnum>().Select(x => new IdentityRole<int> { Name = x.ToString() });
            var rolesInDb = _dbContext.Roles.ToList();

            var rolesToDelete = rolesInDb.Except(enumRolesList, x => x.Name, y => y.Name);
            var rolesToAdd = enumRolesList.Except(rolesInDb, x => x.Name, y => y.Name);

            if (rolesToAdd.Any())
            {
                foreach (var role in rolesToAdd)
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            if (rolesToDelete.Any())
            {
                foreach (var role in rolesToDelete)
                {
                    await _roleManager.DeleteAsync(role);
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _userManager.FindByEmailAsync(_adminCredentials.Email) is null)
            {
                var adminUser = new User
                {
                    UserName = _adminCredentials.Nickname,
                    Email = _adminCredentials.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, _adminCredentials.Password);

                if (!result.Succeeded)
                {
                    throw new System.Exception(Constants.Errors.SeedData.AdminUserWasNotCreated);
                }

                result = await _userManager.AddToRoleAsync(adminUser, RolesEnum.Admin.ToString());

                if (!result.Succeeded)
                {
                    throw new System.Exception(Constants.Errors.SeedData.AdminUserWasNotAddedToAdminRole);
                }
            }
        }
    }
}
