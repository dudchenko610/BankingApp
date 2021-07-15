using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Account.Route)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route(Routes.Account.SignUp)]
        public async Task<IActionResult> SignUp(SignUpAccountView signUpAccountView)
        {
            try
            {
                await _accountService.SignUpAsync(signUpAccountView);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route(Routes.Account.SignIn)]
        public async Task<IActionResult> SignIn(SignInAccountView signInAccountView)
        {
            try
            {
                var tokensView = await _accountService.SignInAsync(signInAccountView);
                return Ok(tokensView);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route(Routes.Account.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailAccountView confirmEmailAccountView)
        {
            try
            {
                var tokensView = await _accountService.ConfirmEmailAsync(confirmEmailAccountView);
                return Ok(tokensView);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
