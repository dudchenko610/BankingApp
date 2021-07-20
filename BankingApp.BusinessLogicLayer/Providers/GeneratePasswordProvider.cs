using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Shared;
using System;
using System.Text;

namespace BankingApp.BusinessLogicLayer.Providers
{
    public class GeneratePasswordProvider : IGeneratePasswordProvider
    {
        public string GeneratePassword()
        {
            string valid = Constants.Password.PasswordValidSymbols;
            var result = new StringBuilder();
            var random = new Random();
            for (int i = default; i < Constants.Password.PasswordLength; i++)
            {
                char value = valid[random.Next(valid.Length)];
                result.Append(value);
            }

            string newPassword = result.ToString();
            return newPassword;
        }
    }
}
