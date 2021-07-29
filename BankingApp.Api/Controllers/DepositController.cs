using Microsoft.AspNetCore.Mvc;
using BankingApp.BusinessLogicLayer.Interfaces;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;
using System;
using Microsoft.AspNetCore.Authorization;
using BankingApp.ViewModels.ViewModels.Deposit;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Deposit.Route)]
    [ApiController]
    public class DepositController : Controller
    {
        private readonly IDepositService _depositService;

        public DepositController(IDepositService bankingService)
        {
            _depositService = bankingService;
        }

        [HttpPost]
        [Authorize]
        [Route(Routes.Deposit.Calculate)]
        public async Task<IActionResult> Calculate(CalculateDepositView requestDepositeData)
        {
            try
            {
                var id = await _depositService.CalculateAsync(requestDepositeData);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route(Routes.Deposit.GetAll)]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var pagedDepositResponse = await _depositService.GetAllAsync(pageNumber, pageSize);
                return Ok(pagedDepositResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route(Routes.Deposit.GetById)]
        public async Task<IActionResult> GetById(int depositId)
        {
            try
            {
                var responseCalculationHistoryViewItem
                    = await _depositService.GetByIdAsync(depositId);
                return Ok(responseCalculationHistoryViewItem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
