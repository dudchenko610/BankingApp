
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Providers
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtConnectionOptions _jwtConnectionOptions;
        private readonly UserManager<User> _userManager;

        public JwtProvider(IOptions<JwtConnectionOptions> jwtConnectionOptions, UserManager<User> userManager)
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

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var symetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConnectionOptions.SecretKey));
            return symetricKey;
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception(Constants.Errors.Authentication.EmailIsRequired);
            }

            var user = await _userManager.FindByEmailAsync(email);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)
            };

            new ClaimsIdentity(claims, Constants.Authentication.Token);
            return claims;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch
            {
                throw new Exception(Constants.Errors.Authentication.SignInPlease);
            }

            return principal;
        }
    }
}
