using BankingApp.UI.Core.Attributes;
using BankingApp.ViewModels.Banking.Account;

namespace BankingApp.UI.Pages.SignInPage
{
    [Unauthorized]
    public partial class SignInPage
    {
        private SignInAuthenticationView _signInView;

        public SignInPage()
        {
            _signInView = new SignInAuthenticationView();
        }

        private void OnFormSubmit()
        { 
        
        }
    }
}
