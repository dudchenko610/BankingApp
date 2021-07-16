using BankingApp.Entities.Entities;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;        
        public User User { get; private set; }
        
        public AuthenticationService(INavigationWrapper navigationWrapper, ILocalStorageService localStorageService)
        {
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
        }

        public async Task InitializeAsync()
        {
            User = await _localStorageService.GetItem<User>("user");
        }

        public Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task LogoutAsync()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            _navigationWrapper.NavigateTo(Routes.Routes.SignInPage);
        }
    }
}
