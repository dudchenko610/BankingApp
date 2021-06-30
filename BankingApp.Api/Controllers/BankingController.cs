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
        [Route(Routes.Banking.CalculateDeposite)]
        public IActionResult CalculateDeposite(RequestCalculateDepositeBankingView model)
        {
            ResponseCalculateDepositeBankingView responseOfDepositeCalculation = _bankingService.CalculateDeposite(model);
            return Ok(responseOfDepositeCalculation);
        }


    }
}
