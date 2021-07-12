using Microsoft.AspNetCore.Mvc;
using BankingApp.BusinessLogicLayer.Interfaces;
using static BankingApp.Shared.Constants;
using System.Threading.Tasks;
using BankingApp.ViewModels.Banking.Calculate;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Banking.BankingRoute)]
    [ApiController]
    public class BankingController : Controller
    {
        private readonly IBankingService _bankingService;

        public BankingController(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }

        [HttpPost]
        [Route(Routes.Banking.CalculateDeposite)]
        public async Task<IActionResult> CalculateDeposite(RequestCalculateDepositeBankingView requestDepositeData)
        {
            var responseOfDepositeCalculation = _bankingService.CalculateDeposite(requestDepositeData);
            int id = await _bankingService?.SaveDepositeCalculationAsync(requestDepositeData, responseOfDepositeCalculation);
            return Ok(id);
        }

        [HttpGet]
        [Route(Routes.Banking.CalculationHistory)]
        public async Task<IActionResult> CalculationHistory()
        {
            var depositeCalculationHistory = await _bankingService.GetDepositesCalculationHistoryAsync();
            return Ok(depositeCalculationHistory);
        }

        [HttpGet]
        [Route(Routes.Banking.CalculationHistoryDetails)]
        public async Task<IActionResult> CalculationHistoryDetails(int depositeHistoryId)
        {
            var responseCalculationHistoryViewItem 
                = await _bankingService.GetDepositeCalculationHistoryDetailsAsync(depositeHistoryId);
            return Ok(responseCalculationHistoryViewItem);
        }
    }
}
