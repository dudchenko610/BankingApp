using Microsoft.AspNetCore.Mvc;
using BankingApp.BusinessLogicLayer.Interfaces;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;
using BankingApp.ViewModels.Banking.Deposit;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route(Routes.Deposit.GetById)]
        public async Task<IActionResult> GetById(int depositeHistoryId)
        {
            try
            {
                var responseCalculationHistoryViewItem
                    = await _depositService.GetByIdAsync(depositeHistoryId);
                return Ok(responseCalculationHistoryViewItem);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
