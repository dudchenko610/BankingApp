using BankingApp.UI.Core.Attributes;
using BankingApp.ViewModels.Banking.Account;

namespace BankingApp.UI.Pages.SignUpPage
{
    [Unauthorized]
    public partial class SignUpPage
    {
        private SignUpAuthenticationView _signUpView;

        public SignUpPage()
        {
            _signUpView = new SignUpAuthenticationView();
        }

        private void OnFormSubmit()
        {

        }
    }
}
