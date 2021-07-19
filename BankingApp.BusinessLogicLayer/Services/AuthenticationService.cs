using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using BankingApp.ViewModels.Banking.Account;
using BankingApp.ViewModels.Banking.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailProvider _emailProvider;
        private readonly IMapper _mapper;
        private readonly ClientConnectionOptions _clientConnectionOptions;

        public AuthenticationService(UserManager<User> userManager,
            IEmailProvider emailProvider,
            IMapper mapper,
            IOptions<ClientConnectionOptions> clientConnectionOptions)
        {
            _userManager = userManager;
            _emailProvider = emailProvider;
            _mapper = mapper;
            _clientConnectionOptions = clientConnectionOptions.Value;
        }

        public async Task ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailView)
        {
            if (string.IsNullOrWhiteSpace(confirmEmailView.Email) || string.IsNullOrWhiteSpace(confirmEmailView.Code))
            {
                throw new Exception(Constants.Errors.Authentication.UserWasNotFound);
            }

            var user = await _userManager.FindByEmailAsync(confirmEmailView.Email);

            byte[] codeDecodeBytes = WebEncoders.Base64UrlDecode(confirmEmailView.Code);
            string codeDecoded = Encoding.UTF8.GetString(codeDecodeBytes);

            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);
            if (!result.Succeeded)
            {
                throw new Exception(Constants.Errors.Authentication.EmailWasNotConfirmed);
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            var existingUser = await _userManager.FindByEmailAsync(signUpAccountView.Email);

            if (existingUser != null)
            {
                throw new Exception(Constants.Errors.Authentication.UserAlreadyExists);
            }

            var user = _mapper.Map<User>(signUpAccountView);
            var result = await _userManager.CreateAsync(user, signUpAccountView.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(x => x.Description).ToList());
                throw new Exception(errors);
            }

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            byte[] tokenGenerateBytes = Encoding.UTF8.GetBytes(code);
            string tokenCode = WebEncoders.Base64UrlEncode(tokenGenerateBytes);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Localhost}{_clientConnectionOptions.Path}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{signUpAccountView.Email}{Constants.Email.ParamCode}{tokenCode}");

            Console.WriteLine("Send email to " + signUpAccountView.Email);

            if (!await _emailProvider.SendEmailAsync(signUpAccountView.Email, Constants.Email.ConfirmEmail, 
                $"{Constants.Email.ConfirmRegistration} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}"))
            {
                await _userManager.DeleteAsync(user);
                throw new Exception(Constants.Errors.Authentication.UerWasNotRegistered);
            }
        }
    }
}
