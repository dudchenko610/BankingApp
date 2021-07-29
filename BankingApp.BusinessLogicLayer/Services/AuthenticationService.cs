using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Entities.Enums;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using BankingApp.ViewModels.ViewModels.Authentication;
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
        private readonly IEmailService _emailProvider;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtProvider;
        private readonly IUserService _userService;
        private readonly ClientConnectionOptions _clientConnectionOptions;

        public AuthenticationService(UserManager<User> userManager,
            IEmailService emailProvider,
            IMapper mapper,
            IJwtService jwtProvider,
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
            var user = await CheckUserForExistenceAsync(confirmEmailView);
            await ConfirmUserEmailAsync(confirmEmailView, user);
        }

        public async Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            var user = await CheckUserForExistenceAsync(signInAccountView);
            var tokensView = await GenerateTokensAsync(user);

            return tokensView;
        }

        public async Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            await CheckUserForExistenceAsync(signUpAccountView);
            var createdUser = await CreateUserAsync(signUpAccountView);
            await SendEmailConfirmationMessageAsync(createdUser);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            var user = await CheckUserForExistenceAsync(resetPasswordAuthenticationView);
            await SendEmailResetPasswordAsync(user);
        }

        public async Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordView)
        {
            var user = await CheckUserForExistenceAsync(resetPasswordView);
            await ResetPasswordAsync(resetPasswordView, user);
        }

        private async Task CheckUserForExistenceAsync(SignUpAuthenticationView signUpAccountView)
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

            result = await _userManager.AddToRoleAsync(user, RolesEnum.Admin.ToString());

            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                throw new Exception(Constants.Errors.Authentication.ClientUserWasNotAddedToClientRole);
            }

            return user;
        }

        private async Task SendEmailConfirmationMessageAsync(User user)
        {
            string emailConfirmationToken = null;

            try
            {
                emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user);

                throw new Exception(e.Message);
            }

            byte[] tokenGenerateBytes = Encoding.UTF8.GetBytes(emailConfirmationToken);
            string tokenCode = WebEncoders.Base64UrlEncode(tokenGenerateBytes);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Localhost}{_clientConnectionOptions.ConfirmPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{user.Email}{Constants.Email.ParamCode}{tokenCode}");

            var messageBody = $"{Constants.Email.ConfirmRegistration} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}";

            if (!await _emailProvider.SendEmailAsync(user.Email, Constants.Email.ConfirmEmail, messageBody))
            {
                await _userManager.DeleteAsync(user);

                throw new Exception(Constants.Errors.Authentication.UserWasNotRegistered);
            }
        }

        private async Task<User> CheckUserForExistenceAsync(SignInAuthenticationView signInAccountView)
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

            return user;
        }

        private async Task<TokensView> GenerateTokensAsync(User user)
        {
            var claims = await _jwtProvider.GetUserClaimsAsync(user.Email);
            string accessToken = _jwtProvider.GenerateAccessToken(claims);

            var tokens = new TokensView
            {
                AccessToken = accessToken
            };

            return tokens;
        }

        private async Task<User> CheckUserForExistenceAsync(ConfirmEmailAuthenticationView confirmEmailView)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailView.Email);

            if (user is null)
            {
                throw new Exception(Constants.Errors.Authentication.UserWasNotFound);
            }

            return user;
        }

        private async Task ConfirmUserEmailAsync(ConfirmEmailAuthenticationView confirmEmailView, User user)
        {
            byte[] codeDecodeBytes = WebEncoders.Base64UrlDecode(confirmEmailView.Code);
            string codeDecoded = Encoding.UTF8.GetString(codeDecodeBytes);

            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);

            if (!result.Succeeded)
            {
                throw new Exception(Constants.Errors.Authentication.EmailWasNotConfirmed);
            }

            await _userManager.UpdateAsync(user);
        }

        private async Task<User> CheckUserForExistenceAsync(ForgotPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            var user = await _userService.GetUserByEmailAsync(resetPasswordAuthenticationView.Email);

            if (user is null)
            {
                throw new Exception(Constants.Errors.Authentication.ErrorWhileSendingMessage);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(RolesEnum.Admin.ToString()))
            {
                throw new Exception(Constants.Errors.Authentication.EmailWasNotDelivered);
            }

            return user;
        }

        private async Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordView, User user)
        {
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordView.Code, resetPasswordView.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(x => x.Description).ToList());

                throw new Exception(errors);
            }
        }

        private async Task<User> CheckUserForExistenceAsync(ResetPasswordAuthenticationView resetPasswordView)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordView.Email);

            if (user == null)
            {
                throw new Exception(Constants.Errors.Authentication.UserWasNotFound);
            }

            return user;
        }

        private async Task SendEmailResetPasswordAsync(User user)
        {
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Localhost}{_clientConnectionOptions.ResetPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{user.Email}{Constants.Email.ParamCode}{HttpUtility.UrlEncode(resetPasswordToken)}");

            var messageBody = $"{Constants.Password.PasswordReset} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}";

            if (!await _emailProvider.SendEmailAsync(user.Email, Constants.Password.PasswordResetHeader, messageBody))
            {
                throw new Exception(Constants.Errors.Authentication.ErrorWhileSendingMessage);
            }
        }
    }
}

