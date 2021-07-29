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
    [ExcludeFromCodeCoverage]
    public class JwtService : IJwtService
    {
        private readonly JwtConnectionOptions _jwtConnectionOptions;
        private readonly UserManager<User> _userManager;

        public JwtService(IOptions<JwtConnectionOptions> jwtConnectionOptions, UserManager<User> userManager)
        {
            _jwtConnectionOptions = jwtConnectionOptions.Value;
            _userManager = userManager;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               issuer: _jwtConnectionOptions.Issuer,
               audience: _jwtConnectionOptions.Audience,
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddMinutes(_jwtConnectionOptions.Lifetime),
               signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

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
            var symetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConnectionOptions.SecretKey));
            
            return symetricKey;
        }
    }
}
