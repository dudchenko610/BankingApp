using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking.Admin;
using Microsoft.AspNetCore.Http;
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
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route(Routes.Admin.GetAll)]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var getAllAdminView = await _adminService.GetAllAsync(pageNumber, pageSize);
                return Ok(getAllAdminView);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route(Routes.Admin.BlockUser)]
        public async Task<IActionResult> BlockUser(BlockUserAdminView blockUserAdminView)
        {
            try
            {
                await _adminService.BlockUserAsync(blockUserAdminView);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
