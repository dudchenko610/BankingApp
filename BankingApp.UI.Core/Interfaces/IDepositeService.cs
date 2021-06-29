using BankingApp.ViewModels.Banking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositeService
    {
        public Task<ResponseCalculateDepositeBankingView> CalculateDepositeSimpleInterestAsync(RequestCalculateDepositeBankingView reqDeposite);
        public Task<ResponseCalculateDepositeBankingView> CalculateDepositeCompoundInterestAsync(RequestCalculateDepositeBankingView reqDeposite);
    }
}
