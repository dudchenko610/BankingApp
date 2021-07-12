using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking;
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

        private delegate (decimal MonthSum, int Percents) CalculationFormula(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo);

        public BankingService(IMapper mapper, IDepositeHistoryRepository depositeHistoryRepository)
        {
            _mapper = mapper;
            _depositeHistoryRepository = depositeHistoryRepository;
        }

        public ResponseCalculateDepositeBankingView CalculateDeposite(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            var respDepositeInfo = new ResponseCalculateDepositeBankingView();
            CalculationFormula calculationFormula = GetCalculationFormula(reqDepositeCalcInfo);

            for (int i = 1; i <= reqDepositeCalcInfo.MonthsCount; i++)
            {
                var res = calculationFormula(i, reqDepositeCalcInfo);
                respDepositeInfo.PerMonthInfos.Add(new ResponseCalculateDepositeBankingViewItem
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(res.MonthSum, 2),
                    Percents = res.Percents
                });
            }
            return respDepositeInfo;
        }

        public async Task<ResponseCalculationHistoryBankingViewItem> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId)
        {
            // maybe in future, here should be checking for null and throwing an exception
            var depositeHistoryWithItems = await _depositeHistoryRepository.GetDepositeHistoryWithItemsAsync(depositeHistoryId);
            var responseCalculationHistoryViewItem = _mapper.Map<DepositeHistory, ResponseCalculationHistoryBankingViewItem>(depositeHistoryWithItems);
            return responseCalculationHistoryViewItem;
        }
       
        public async Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync()
        {
            IList<DepositeHistory> depositesHistoryFromDb = await _depositeHistoryRepository.GetAsync();

            var response = new ResponseCalculationHistoryBankingView();
            response.DepositesHistory = _mapper.Map<IList<DepositeHistory>, IList<ResponseCalculationHistoryBankingViewItem>>(depositesHistoryFromDb);

            return response;
        }
       
        public async Task<int> SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo,
            ResponseCalculateDepositeBankingView depositeCalculation)
        {
            var depositeHistory = _mapper.Map<DepositeHistory>(reqDepositeCalcInfo);
            depositeHistory.CalulationDateTime = System.DateTime.Now;
            depositeHistory.DepositeHistoryItems
                = _mapper.Map<IList<ResponseCalculateDepositeBankingViewItem>, IList<DepositeHistoryItem>>(depositeCalculation.PerMonthInfos);

            int savedId = await _depositeHistoryRepository.AddAsync(depositeHistory);
            return savedId;
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

        private (decimal MonthSum, int Percents) CalculateSimpleInterestDepositePerMonth(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            // An = A(1 + (n / 12) * (P / 100))
            float percentsDevidedBy1200 = (float) reqDepositeCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal)(1.0f + monthNumber * percentsDevidedBy1200);
            int percents = (int)((monthNumber / 12.0f) * (float)reqDepositeCalcInfo.Percents);
            return (monthSum, percents);
        }

        private (decimal MonthSum, int Percents) CalculateCompoundInterestDepositePerMonth(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            // An = A(1 + (P / 1200)) ^ (n)
            float percentsDevidedBy1200 = (float) reqDepositeCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal)Math.Pow(1.0 + percentsDevidedBy1200, monthNumber);
            int percents = (int)(((monthSum - reqDepositeCalcInfo.DepositeSum) / reqDepositeCalcInfo.DepositeSum) * 100.0m);
            return (monthSum, percents);
        }
    }
}
