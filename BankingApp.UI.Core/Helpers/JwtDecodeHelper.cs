using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace BankingApp.UI.Core.Helpers
{
    /// <summary>
    /// Helps to get list of claims from access token.
    /// </summary>
    public static class JwtDecodeHelper
    {
        /// <summary>
        /// Parses list of claims from access token.
        /// </summary>
        /// <param name="jwt">Access token.</param>
        /// <returns>List of <see cref="Claim"/>s.</returns>
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
