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
    /// <summary>
    /// Allows the user to provide authentication operations with account.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailProvider;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtProvider;
        private readonly ClientConnectionOptions _clientConnectionOptions;

        /// <summary>
        /// Creates instance of <see cref="AuthenticationService"/>.
        /// </summary>
        /// <param name="userManager">Allows make operations with users using ASP NET Identity.</param>
        /// <param name="emailProvider">Allows to send email messages.</param>
        /// <param name="mapper">Allows to map models.</param>
        /// <param name="jwtProvider">Allows to generate access token.</param>
        /// <param name="userService">Allows to provide operations with users.</param>
        /// <param name="clientConnectionOptions">Contains view model with client connection options mapped from appsettings.</param>
        public AuthenticationService(UserManager<User> userManager,
            IEmailService emailProvider,
            IMapper mapper,
            IJwtService jwtProvider,
            IOptions<ClientConnectionOptions> clientConnectionOptions)
        {
            _userManager = userManager;
            _emailProvider = emailProvider;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _clientConnectionOptions = clientConnectionOptions.Value;
        }

        /// <summary>
        /// Confirms user's email in system
        /// </summary>
        /// <exception cref="Exception">When user confirmation fails.</exception>
        /// <param name="confirmEmailView">View model containing user's email and confirmation token.</param>
        public async Task ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailView)
        {
            var user = await GetUserIfExistsAsync(confirmEmailView.Email, Constants.Errors.Authentication.UserWasNotFound);
            await ConfirmUserEmailAsync(confirmEmailView, user);
        }

        /// <summary>
        /// Makes user logged in the system.
        /// </summary>
        /// <param name="signInAccountView">View model containing data needed to sign in user.</param>
        /// <exception cref="Exception">If there is no such user or invalid credentials</exception>
        /// <returns>View model containing access token.</returns>
        public async Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            var user = await GetUserIfExistsAsync(signInAccountView);
            CheckIfUserIsBlocked(user);
            var tokensView = await GenerateTokensAsync(user);

            return tokensView;
        }

        /// <summary>
        /// Makes user registered the system.
        /// </summary>
        /// <param name="signUpAccountView"></param>
        /// <exception cref="Exception">When occurred error while creating user or sending email message.</exception>
        public async Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            await CheckUserForExistenceAsync(signUpAccountView);
            var createdUser = await CreateUserAsync(signUpAccountView);
            await SendEmailConfirmationMessageAsync(createdUser);
        }

        /// <summary>
        /// Provides user with ability to reset password.
        /// </summary>
        /// <param name="resetPasswordAuthenticationView">View model containing user's email.</param>
        /// <exception cref="Exception">If there is no such user or email sending failed.</exception>
        public async Task ForgotPasswordAsync(ForgotPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            var user = await GetUserIfExistsAsync(resetPasswordAuthenticationView.Email, Constants.Errors.Authentication.ErrorWhileSendingMessage);
            await SendEmailResetPasswordAsync(user);
        }

        /// <summary>
        /// Replaces old password with new.
        /// </summary>
        /// <param name="resetPasswordView">View model containing email, reset password token and new password.</param>
        /// <exception cref="Exception">If there is no such user or reset token is wrong.</exception>
        public async Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordView)
        {
            var user = await GetUserIfExistsAsync(resetPasswordView.Email, Constants.Errors.Authentication.UserWasNotFound);
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

            result = await _userManager.AddToRoleAsync(user, RolesEnum.Client.ToString());
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
            callbackUrl.Append($"{_clientConnectionOptions.Url}{_clientConnectionOptions.ConfirmPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{user.Email}{Constants.Email.ParamCode}{tokenCode}");

            var messageBody = $"{Constants.Email.ConfirmRegistration} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}";

            if (!await _emailProvider.SendEmailAsync(user.Email, Constants.Email.ConfirmEmail, messageBody))
            {
                await _userManager.DeleteAsync(user);

                throw new Exception(Constants.Errors.Authentication.UserWasNotRegistered);
            }
        }

        private async Task<User> GetUserIfExistsAsync(SignInAuthenticationView signInAccountView)
        {
            var user = await _userManager.FindByEmailAsync(signInAccountView.Email);

            if (user is null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, signInAccountView.Password))
            {
                throw new Exception(Constants.Errors.Authentication.InvalidNicknameOrPassword);
            }
            return user;
        }

        private void CheckIfUserIsBlocked(User user)
        {
            if (user.IsBlocked)
            {
                throw new Exception(Constants.Errors.Authentication.UserIsBlocked);
            }
        }

        private async Task<User> GetUserIfExistsAsync(string email, string errorMessage)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new Exception(errorMessage);
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

        private async Task SendEmailResetPasswordAsync(User user)
        {
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = new StringBuilder();
            callbackUrl.Append($"{_clientConnectionOptions.Url}{_clientConnectionOptions.ResetPath}");
            callbackUrl.Append($"{Constants.Email.ParamEmail}{user.Email}{Constants.Email.ParamCode}{HttpUtility.UrlEncode(resetPasswordToken)}");

            var messageBody = $"{Constants.Password.PasswordReset} {Constants.Email.OpenTagLink}{callbackUrl}{Constants.Email.CloseTagLink}";

            if (!await _emailProvider.SendEmailAsync(user.Email, Constants.Password.PasswordResetHeader, messageBody))
            {
                throw new Exception(Constants.Errors.Authentication.ErrorWhileSendingMessage);
            }
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

