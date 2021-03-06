using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Authentication.Route)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route(Routes.Authentication.SignUp)]
        public async Task<IActionResult> SignUp(SignUpAuthenticationView signUpAccountView)
        {

            try
            {
                await _authenticationService.SignUpAsync(signUpAccountView);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route(Routes.Authentication.SignIn)]
        public async Task<IActionResult> SignIn(SignInAuthenticationView signInAccountView)
        {

            try
            {
                var tokensView = await _authenticationService.SignInAsync(signInAccountView);

                return Ok(tokensView);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route(Routes.Authentication.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {

            try
            {
                await _authenticationService.ConfirmEmailAsync(confirmEmailAccountView);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route(Routes.Authentication.ResetPassword)]
        public async Task<IActionResult> ResetPassword(ResetPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            try
            {
                await _authenticationService.ResetPasswordAsync(resetPasswordAuthenticationView);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route(Routes.Authentication.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView)
        {
            try
            {
                await _authenticationService.ForgotPasswordAsync(forgotPasswordAuthenticationView);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
