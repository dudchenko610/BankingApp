using Microsoft.AspNetCore.Mvc;
using BankingApp.BusinessLogicLayer.Interfaces;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;
using BankingApp.ViewModels.Banking.Deposit;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Banking.DepositRoute)]
    [ApiController]
    public class DepositController : Controller
    {
        private readonly IDepositService _depositService;

        public DepositController(IDepositService bankingService)
        {
            _depositService = bankingService;
        }

        [HttpPost]
        [Route(Routes.Banking.Calculate)]
        public async Task<IActionResult> Calculate(CalculateDepositView requestDepositeData)
        {
            var id = await _depositService.CalculateAsync(requestDepositeData);
            return Ok(id);
        }

        [HttpGet]
        [Route(Routes.Banking.GetAll)]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var pagedDepositResponse = await _depositService.GetAllAsync(pageNumber, pageSize);
            return Ok(pagedDepositResponse);
        }

        [HttpGet]
        [Route(Routes.Banking.GetById)]
        public async Task<IActionResult> GetById(int depositeHistoryId)
        {
            var responseCalculationHistoryViewItem 
                = await _depositService.GetByIdAsync(depositeHistoryId);
            return Ok(responseCalculationHistoryViewItem);
        }
    }
}
