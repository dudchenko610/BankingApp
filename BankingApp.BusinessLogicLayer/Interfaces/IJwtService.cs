using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Allows users to provide operations with their access token.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Allows users to get their claims by email.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns>Collection with created claims.</returns>
        Task<IEnumerable<Claim>> GetUserClaimsAsync(string email);

        /// <summary>
        /// Generates new access by given user's claims
        /// </summary>
        /// <param name="claims">User's claims</param>
        /// <returns>Access token</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}
