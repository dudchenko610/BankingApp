using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Enums;
using BankingApp.ViewModels.Pagination;
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

        public async Task<int> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            var depositeHistory = new DepositeHistory();
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
       
        public async Task<ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>> GetDepositesCalculationHistoryAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new Exception(Constants.Errors.Page.IncorrectPageNumberFormat);

            if (pageSize < 1)
                throw new Exception(Constants.Errors.Page.IncorrectPageSizeFormat);

            (IList<DepositeHistory> DepositeHistory, int TotalCount) 
                = await _depositeHistoryRepository.GetDepositesHistoryPagedAsync((pageNumber - 1) * pageSize, pageSize);

            var pagedResponse = new ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>
            {
                Data = _mapper.Map<IList<DepositeHistory>, IList<ResponseCalculationHistoryBankingViewItem>>(DepositeHistory),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = TotalCount
            };

            return pagedResponse;
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
