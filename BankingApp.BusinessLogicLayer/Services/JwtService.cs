using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    /// <summary>
    /// Allows users to provide operations with their access token.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Creates instance of <see cref="JwtService"/>
        /// </summary>
        /// <param name="jwtOptions">Contains view model with jwt options mapped from appsettings</param>
        /// <param name="userManager">Allows make operations with users using ASP NET Identity.</param>
        public JwtService(IOptions<JwtOptions> jwtOptions, UserManager<User> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        /// <summary>
        /// Generates new access by given user's claims
        /// </summary>
        /// <param name="claims">User's claims</param>
        /// <returns>Access token</returns>
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               issuer: _jwtOptions.Issuer,
               audience: _jwtOptions.Audience,
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddMinutes(_jwtOptions.Lifetime),
               signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

        /// <summary>
        /// Allows users to get their claims by email.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <exception cref="Exception">If user has more or less than 1 role</exception>
        /// <returns>Collection with created claims.</returns>
        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception(Constants.Errors.Authentication.EmailIsRequired);
            }

            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count != 1)
            {
                throw new Exception(Constants.Errors.Authentication.UserShouldBelongToOneRole);
            }

            var role = roles[0];

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            return claims;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var symetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey));
            
            return symetricKey;
        }
    }
}
