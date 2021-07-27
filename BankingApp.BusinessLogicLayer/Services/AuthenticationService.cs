using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Entities.Enums;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using BankingApp.ViewModels.Banking.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailProvider _emailProvider;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserService _userService;
        private readonly ClientConnectionOptions _clientConnectionOptions;

        public AuthenticationService(UserManager<User> userManager,
            IEmailProvider emailProvider,
            IMapper mapper,
            IJwtProvider jwtProvider,
            IUserService userService,
            IOptions<ClientConnectionOptions> clientConnectionOptions)
        {
            _userManager = userManager;
            _emailProvider = emailProvider;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _userService = userService;
            _clientConnectionOptions = clientConnectionOptions.Value;
        }

        public async Task ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailView)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailView.Email);

            if (user is null)
            {
                throw new Exception(Constants.Errors.Authentication.UserWasNotFound);
            }

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
            var user = await _userManager.FindByEmailAsync(signInAccountView.Email);

            if (user is null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, signInAccountView.Password))
            {
                throw new Exception(Constants.Errors.Authentication.InvalidNicknameOrPassword);
            }

            if (user.IsBlocked)
            {
                throw new Exception(Constants.Errors.Authentication.UserIsBlocked);
            }

            var claims = await _jwtProvider.GetUserClaimsAsync(user.Email);
            string accessToken = _jwtProvider.GenerateAccessToken(claims);

            var tokens = new TokensView
            {
                AccessToken = accessToken
            };

            return tokens;
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
                await _userManager.DeleteAsync(user);
                string errors = string.Join("\n", result.Errors.Select(x => x.Description).ToList());
                throw new Exception(errors);
            }

            result = await _userManager.AddToRoleAsync(user, RolesEnum.Client.ToString());
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new System.Exception(Constants.Errors.Authentication.ClientUserWasNotAddedToAdminRole);
            }

            string code = null;
            try
            {
                code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception(e.Message);
            }

            byte[] tokenGenerateBytes = Encoding.UTF8.GetBytes(code);
            string tokenCode = WebEncoders.Base64UrlEncode(tokenGenerateBytes);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Localhost}{_clientConnectionOptions.ConfirmPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{signUpAccountView.Email}{Constants.Email.ParamCode}{tokenCode}");

            if (!await _emailProvider.SendEmailAsync(signUpAccountView.Email, Constants.Email.ConfirmEmail, 
                $"{Constants.Email.ConfirmRegistration} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}"))
            {
                await _userManager.DeleteAsync(user);
                throw new Exception(Constants.Errors.Authentication.UserWasNotRegistered);
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordAuthenticationView.Email))
            {
                throw new Exception(Constants.Errors.Authentication.EmailIsRequired);
            }

            var user = await _userService.GetUserByEmailAsync(resetPasswordAuthenticationView.Email);
            if (user is null)
            {
                throw new Exception(Constants.Errors.Authentication.ErrorWhileResetPaswword); // we don't want to show lack of our user in this particular case
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(RolesEnum.Admin.ToString()))
            {
                throw new Exception(Constants.Errors.Authentication.ErrorWhileResetPaswword); // we don't want to show lack of our user in this particular case
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Localhost}{_clientConnectionOptions.ResetPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{resetPasswordAuthenticationView.Email}{Constants.Email.ParamCode}{HttpUtility.UrlEncode(resetPasswordToken)}");

            var emailBody = $"{Constants.Password.PasswordReset} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}";

            if (!await _emailProvider.SendEmailAsync(resetPasswordAuthenticationView.Email, Constants.Password.PasswordResetHeader, emailBody))
            {
                throw new Exception(Constants.Errors.Authentication.ErrorWhileSendingMessage);
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordView)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordView.Email);
            if (user == null)
            {
                throw new Exception(Constants.Errors.Authentication.UserWasNotFound);
            }
            
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordView.Code, resetPasswordView.Password);
            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(x => x.Description).ToList());
                throw new Exception(errors);
            }
        }


        private async Task CheckForUserExistenceAsync(SignUpAuthenticationView signUpAccountView)
        {
            var existingUser = await _userManager.FindByEmailAsync(signUpAccountView.Email);

            if (existingUser != null)
            {
                throw new Exception(Constants.Errors.Authentication.UserAlreadyExists);
            }
        }

        private async Task<User> CreateUserAsync(SignUpAuthenticationView signUpAccountView)
        {
            var user = _mapper.Map<User>(signUpAccountView);
            var result = await _userManager.CreateAsync(user, signUpAccountView.Password);

            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                string errors = string.Join("\n", result.Errors.Select(x => x.Description).ToList());
                throw new Exception(errors);
            }

            return user;
        }

        private async Task SendEmailWithConfirmationTokenAsync(User user)
        {
            string confirmationToken = null;
            try
            {
                confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user);
                throw e;
            }

            byte[] tokenGenerateBytes = Encoding.UTF8.GetBytes(confirmationToken);
            string tokenCode = WebEncoders.Base64UrlEncode(tokenGenerateBytes);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Localhost}{_clientConnectionOptions.ConfirmPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{user.Email}{Constants.Email.ParamCode}{tokenCode}");

            var confirmEmailLink = $"{Constants.Email.ConfirmRegistration} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}";

            if (!await _emailProvider.SendEmailAsync(user.Email, Constants.Email.ConfirmEmail, confirmEmailLink))
            {
                await _userManager.DeleteAsync(user);
                throw new Exception(Constants.Errors.Authentication.UserWasNotRegistered);
            }
        }
    }
}
