using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IJwtProvider
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync(string email);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        SymmetricSecurityKey GetSymmetricSecurityKey();
        ClaimsPrincipal ValidateToken(string token);
    }
}
