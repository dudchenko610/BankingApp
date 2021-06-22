using Microsoft.AspNetCore.Mvc;

using BusinessLogicLayer.Facade;
using Shared.ViewModels.Banking;

using static Shared.Constants;

namespace CrazyTasks.Controllers
{
    [Route("[controller]")]
    public class BankingController : Controller
    {
        private IBankingService _bankingService;
        public BankingController(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }

        [HttpPost]
        [Route(Routes.Banking.CALC_DEPOSITE)]
        public IActionResult CalcDeposite(DepositeInputData model)
        {
            DepositeOutputData depositeOutputData = _bankingService.CalculateDeposite(model);
            return Ok(depositeOutputData);
        }

    }
}
