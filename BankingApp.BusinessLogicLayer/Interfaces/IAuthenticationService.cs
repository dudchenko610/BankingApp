﻿using BankingApp.ViewModels.Banking.Account;
using BankingApp.ViewModels.Banking.Authentication;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView);
        Task SignUpAsync(SignUpAuthenticationView signUpAccountView);
        Task ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);
        Task ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView);
        Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView);
    }
}
