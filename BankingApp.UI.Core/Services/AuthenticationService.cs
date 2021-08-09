using BankingApp.UI.Core.Helpers;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Authentication;
using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.Services
{
    /// <summary>
    ///  Allows the user to provide authentication operations with account.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpService _httpService;

        private IList<Claim> _claims;

        /// <summary>
        /// Used to get actual information about access token.
        /// </summary>
        public TokensView TokensView { get; private set; }
        /// <summary>
        /// Gets list of roles.
        /// </summary>
        /// <returns>List of role names.</returns>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// Creates instance of <see cref="AuthenticationService"/>.
        /// </summary>
        /// <param name="navigationWrapper">Allows to navigate the application routes.</param>
        /// <param name="localStorageService">Allows to perform read / write operations with browser local storage.</param>
        /// <param name="httpService">Allows send HTTP request to server.</param>
        public AuthenticationService(INavigationWrapper navigationWrapper, 
            ILocalStorageService localStorageService,
            IHttpService httpService)
        {
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
            _httpService = httpService;

            _claims = new List<Claim>();
        }

        /// <summary>
        /// Decodes access token from local storage.
        /// </summary>
        public async Task InitializeAsync()
        {
            TokensView = await _localStorageService.GetItemAsync<TokensView>(Constants.Constants.Authentication.TokensView);
            ExtractClaimsFromToken();
        }

        /// <summary>
        /// Makes user registered the system.
        /// </summary>
        /// <param name="signUpAccountView">View model containing data needed to register user.</param>
        /// <returns>True if operation succeed and false otherwise./></returns>
        public async Task<bool> SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            return await _httpService.PostAsync<SignUpAuthenticationView>($"{Routes.Authentication.Route}/{Routes.Authentication.SignUp}", signUpAccountView, false);
        }

        /// <summary>
        /// Confirms user's email in system.
        /// </summary>
        /// <param name="confirmEmailAccountView">View model containing user's email and confirmation token.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        public async Task<bool> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            return await _httpService.PostAsync<ConfirmEmailAuthenticationView>($"{Routes.Authentication.Route}/{Routes.Authentication.ConfirmEmail}", confirmEmailAccountView, false);
        }

        /// <summary>
        /// Makes user logged in the system.
        /// </summary>
        /// <param name="signInAccountView">View model containing data needed to sign in user.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        public async Task<bool> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            var tokensView = await _httpService.PostAsync<TokensView, SignInAuthenticationView>($"{Routes.Authentication.Route}/{Routes.Authentication.SignIn}", signInAccountView, false);
            await _localStorageService.SetItemAsync(Constants.Constants.Authentication.TokensView, tokensView);
            TokensView = tokensView;
            ExtractClaimsFromToken();

            return tokensView != null;
        }

        /// <summary>
        /// Logouts user.
        /// </summary>
        public async Task LogoutAsync()
        {
            TokensView = null;
            _claims = new List<Claim>();
            IsAdmin = false;
            await _localStorageService.RemoveItemAsync(Constants.Constants.Authentication.TokensView);
            _navigationWrapper.NavigateTo(Constants.Constants.Routes.SignInPage);
        }

        /// <summary>
        /// Replaces old password with new.
        /// </summary>
        /// <param name="resetPasswordAuthenticationView">View model containing email, reset password token and new password.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        public async Task<bool> ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            return await _httpService.PostAsync<ResetPasswordAuthenticationView>($"{Routes.Authentication.Route}/{Routes.Authentication.ResetPassword}", resetPasswordAuthenticationView, false);
        }

        /// <summary>
        /// Provides user with ability to reset password.
        /// </summary>
        /// <param name="forgotPasswordAuthenticationView">View model containing user's email.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView)
        {
            return await _httpService.PostAsync<ForgotPasswordAuthenticationView>($"{Routes.Authentication.Route}/{Routes.Authentication.ForgotPassword}", forgotPasswordAuthenticationView, false);
        }

        /// <summary>
        /// Gets list of roles.
        /// </summary>
        /// <returns>List of role names.</returns>
        public IList<string> GetRoles()
        {
            return _claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Select(x => x.Value).ToList();
        }

        private void ExtractClaimsFromToken()
        {
            if (TokensView == null)
            {
                _claims = new List<Claim>();
            }
            else
            {
                _claims = JwtDecodeHelper.ParseClaimsFromJwt(TokensView.AccessToken).ToList();
                IsAdmin = _claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType && x.Value == Roles.Admin.ToString()) != null;
            }
        }
    }
}
