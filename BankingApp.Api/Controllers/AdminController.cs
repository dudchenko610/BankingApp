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
    /// <summary>
    /// Controller that contains endpoints for admin user
    /// </summary>
    [Route(Routes.Admin.Route)]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Assigns needed services to private fields
        /// </summary>
        /// <param name="userService">User service dependecy</param>
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets paged users, available only for admin
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Current page size</param>
        /// <returns>Returns OkObjectResult with paged users or, if operation fails, BadRequestObjectResult with error message</returns>
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

        /// <summary>
        /// Blocks user, available only for admin
        /// </summary>
        /// <param name="blockUserAdminView">Model containing UserId and type of blocking (block / unblock)</param>
        /// <returns>Returns OkObjectResult with paged users or, if operation fails, BadRequestObjectResult with error message</returns>
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
