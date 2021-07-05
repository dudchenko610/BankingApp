﻿using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingHistoryService
    {
        Task SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo, 
            ResponseCalculateDepositeBankingView depositeCalculation);

        Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync();
    }
}