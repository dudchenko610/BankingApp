﻿using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Interfaces;
using static Shared.Constants;
using ViewModels.Banking;

namespace BankingApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankingController : Controller
    {
        private readonly IBankingService _bankingService;

        public BankingController(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }

        [HttpPost]
        [Route(Routes.Banking.CALCULATE_DEPOSITE)]
        public IActionResult CalculateDeposite(RequestCalculateDepositeBankingView model)
        {
            ResponseCalculateDepositeBankingView responseOfDepositeCalculation = _bankingService.CalculateDeposite(model);
            return Ok(responseOfDepositeCalculation);
        }
    }
}