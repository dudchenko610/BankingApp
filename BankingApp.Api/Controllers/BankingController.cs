using Microsoft.AspNetCore.Mvc;
using BankingApp.BusinessLogicLayer.Interfaces;
using static BankingApp.Shared.Constants;
using BankingApp.ViewModels.Banking;

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
        [Route(Routes.Banking.CalculateDepositeSimpleInterest)]
        public IActionResult CalculateDepositeSimpleInterest(RequestCalculateDepositeBankingView model)
        {
            ResponseCalculateDepositeBankingView responseOfDepositeCalculation = _bankingService.CalculateDepositeSimpleInterest(model);
            return Ok(responseOfDepositeCalculation);
        }

        [HttpPost]
        [Route(Routes.Banking.CalculateDepositeCompoundInterest)]
        public IActionResult CalculateDepositeCompoundInterest(RequestCalculateDepositeBankingView model)
        {
            ResponseCalculateDepositeBankingView responseOfDepositeCalculation = _bankingService.CalculateDepositeCompoundInterest(model);
            return Ok(responseOfDepositeCalculation);
        }
    }
}
