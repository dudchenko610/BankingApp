using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Admin.Route)]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = Constants.Roles.Admin)]
        [Route(Routes.Admin.GetAll)]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var getAllAdminView = await _userService.GetAllAsync(pageNumber, pageSize);

                return Ok(getAllAdminView);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.Admin)]
        [Route(Routes.Admin.BlockUser)]
        public async Task<IActionResult> BlockUser(BlockUserAdminView blockUserAdminView)
        {
            try
            {
                await _userService.BlockAsync(blockUserAdminView);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
