using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class BankingService : IBankingService
    {
        private readonly IMapper _mapper;
        private readonly IDepositeHistoryRepository _depositeHistoryRepository;

        private delegate (decimal MonthSum, float Percents) CalculationFormula(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo);

        public BankingService(IMapper mapper, IDepositeHistoryRepository depositeHistoryRepository)
        {
            _mapper = mapper;
            _depositeHistoryRepository = depositeHistoryRepository;
        }

        public async Task<int> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            var depositeHistory = _mapper.Map<RequestCalculateDepositeBankingView, DepositeHistory>(reqDepositeCalcInfo);
            
            depositeHistory.CalulationDateTime = System.DateTime.Now;
            CalculationFormula calculationFormula = GetCalculationFormula(reqDepositeCalcInfo);

            for (int i = 1; i <= reqDepositeCalcInfo.MonthsCount; i++)
            {
                var res = calculationFormula(i, reqDepositeCalcInfo);
                depositeHistory.DepositeHistoryItems.Add(new DepositeHistoryItem
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(res.MonthSum, 2),
                    Percents = res.Percents
                });
            }
            
            int savedId = await _depositeHistoryRepository.AddAsync(depositeHistory);
            return savedId;
        }

        public async Task<ResponseCalculationHistoryDetailsBankingView> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId)
        {
            var depositeHistoryWithItems = await _depositeHistoryRepository.GetDepositeHistoryWithItemsAsync(depositeHistoryId);
            if (depositeHistoryWithItems == null)
                throw new Exception(Constants.Errors.Banking.IncorrectDepositeHistoryId);

            var responseCalculationHistoryViewItem = _mapper.Map<DepositeHistory, ResponseCalculationHistoryDetailsBankingView>(depositeHistoryWithItems);
            return responseCalculationHistoryViewItem;
        }

        public async Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync()
        {
            IList<DepositeHistory> depositesHistoryFromDb = await _depositeHistoryRepository.GetAsync();

            var response = new ResponseCalculationHistoryBankingView();
            response.DepositesHistory = _mapper.Map<IList<DepositeHistory>, IList<ResponseCalculationHistoryBankingViewItem>>(depositesHistoryFromDb);

            return response;
        }

        private CalculationFormula GetCalculationFormula(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            switch (reqDepositeCalcInfo.CalculationFormula)
            {
                case DepositeCalculationFormulaEnumView.SimpleInterest:
                    return CalculateSimpleInterestDepositePerMonth;
                default:
                    return CalculateCompoundInterestDepositePerMonth;
            }
        }

        private (decimal MonthSum, float Percents) CalculateSimpleInterestDepositePerMonth(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            // An = A(1 + (n / 12) * (P / 100))
            float percentsDevidedBy1200 = (float) reqDepositeCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal)(1.0f + monthNumber * percentsDevidedBy1200);
            float percents = (float) decimal.Round((decimal)(monthNumber / 12.0f) * reqDepositeCalcInfo.Percents, 2);
            return (monthSum, percents);
        }

        private (decimal MonthSum, float Percents) CalculateCompoundInterestDepositePerMonth(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            // An = A(1 + (P / 1200)) ^ (n)
            float percentsDevidedBy1200 = (float) reqDepositeCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal)Math.Pow(1.0 + percentsDevidedBy1200, monthNumber);
            float percents = (float)decimal.Round(((monthSum - reqDepositeCalcInfo.DepositeSum) / reqDepositeCalcInfo.DepositeSum) * 100.0m, 2);
            return (monthSum, percents);
        }
    }
}
