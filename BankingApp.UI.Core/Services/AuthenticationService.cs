using BankingApp.Entities.Entities;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpService _httpService;
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;        
        public User User { get; private set; }
        
        public AuthenticationService(IHttpService httpService, INavigationWrapper navigationWrapper, ILocalStorageService localStorageService)
        {
            _httpService = httpService;
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
