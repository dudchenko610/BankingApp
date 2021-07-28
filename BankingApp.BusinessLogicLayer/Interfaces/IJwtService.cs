using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IJwtService
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync(string email);
        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}
