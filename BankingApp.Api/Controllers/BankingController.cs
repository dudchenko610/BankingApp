using Microsoft.AspNetCore.Mvc;
using BankingApp.BusinessLogicLayer.Interfaces;
using static BankingApp.Shared.Constants;
using BankingApp.ViewModels.Banking;
using System.Threading.Tasks;

namespace BankingApp.Api.Controllers
{
    [Route(Routes.Banking.BankingRoute)]
    [ApiController]
    public class BankingController : Controller
    {
        private readonly IBankingCalculationService _bankingCalculationService;
        private readonly IBankingHistoryService _bankingHistoryService;

        public BankingController(IBankingCalculationService bankingService, IBankingHistoryService bankingHistoryService)
        {
            _bankingCalculationService = bankingService;
            _bankingHistoryService = bankingHistoryService;
        }

        [HttpPost]
        [Route(Routes.Banking.CalculateDeposite)]
        public async Task<IActionResult> CalculateDeposite(RequestCalculateDepositeBankingView requestDepositeData)
        {
            var responseOfDepositeCalculation = _bankingCalculationService.CalculateDeposite(requestDepositeData);
            await _bankingHistoryService?.SaveDepositeCalculationAsync(requestDepositeData, responseOfDepositeCalculation);
            return Ok(responseOfDepositeCalculation);
        }

        [HttpGet]
        [Route(Routes.Banking.CalculationHistory)]
        public async Task<IActionResult> CalculationHistory()
        {
            var depositeCalculationHistory = await _bankingHistoryService.GetDepositesCalculationHistoryAsync();
            return Ok(depositeCalculationHistory);
        }

        [HttpGet]
        [Route(Routes.Banking.CalculationHistoryDetails)]
        public async Task<IActionResult> CalculationHistoryDetails(int depositeHistoryId)
        {
            var responseCalculationHistoryViewItem 
                = await _bankingHistoryService.GetDepositeCalculationHistoryDetailsAsync(depositeHistoryId);
            return Ok(responseCalculationHistoryViewItem);
        }
    }
}
