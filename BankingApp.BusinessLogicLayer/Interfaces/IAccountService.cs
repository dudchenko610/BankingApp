using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IAccountService
    {
        Task<TokensView> SignInAsync(SignInAccountView signInAccountView);
        Task SignUpAsync(SignUpAccountView signUpAccountView);
        Task<TokensView> ConfirmEmailAsync(ConfirmEmailAccountView confirmEmailAccountView);
    }
}
