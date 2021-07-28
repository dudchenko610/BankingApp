using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        
        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public int GetSignedInUserId()
        {
            var userIdTextRepresentation = _httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            try
            {
                return int.Parse(userIdTextRepresentation);
            }
            catch
            {
                return -1;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
