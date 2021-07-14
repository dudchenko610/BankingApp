using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class DepositService : IDepositService
    {
        private readonly IMapper _mapper;
        private readonly IDepositRepository _depositRepository;

        private delegate (decimal MonthSum, float Percents) CalculationFormula(int monthNumber, CalculateDepositView reqDepositeCalcInfo);

        public DepositService(IMapper mapper, IDepositRepository depositeHistoryRepository)
        {
            _mapper = mapper;
            _depositRepository = depositeHistoryRepository;
        }

        public async Task<int> CalculateAsync(CalculateDepositView requestDepositCalcInfo)
        {
            var depositModel = _mapper.Map<CalculateDepositView, Deposit>(requestDepositCalcInfo);
            
            depositModel.CalсulationDateTime = System.DateTime.Now;
            CalculationFormula calculationFormula = GetCalculationFormula(requestDepositCalcInfo);

            for (int i = 1; i <= requestDepositCalcInfo.MonthsCount; i++)
            {
                var res = calculationFormula(i, requestDepositCalcInfo);
                depositModel.MonthlyPayments.Add(new MonthlyPayment
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(res.MonthSum, 2),
                    Percents = res.Percents
                });
            }
            
            int savedId = await _depositRepository.AddAsync(depositModel);
            return savedId;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId)
        {
            var depositWithItems = await _depositRepository.GetDepositWithItemsByIdAsync(depositeHistoryId);
            if (depositWithItems == null)
                throw new Exception(Constants.Errors.Deposit.IncorrectDepositeHistoryId);

            var depositWithItemsView = _mapper.Map<Deposit, GetByIdDepositView>(depositWithItems);
            return depositWithItemsView;
        }

        public async Task<GetAllDepositView> GetAllAsync()
        {
            IList<Deposit> depositFromDb = await _depositRepository.GetAsync();

            var response = new GetAllDepositView();
            response.DepositItems = _mapper.Map<IList<Deposit>, IList<DepositGetAllDepositViewItem>>(depositFromDb);

            return response;
        }

        private CalculationFormula GetCalculationFormula(CalculateDepositView reqDepositeCalcInfo)
        {
            switch (reqDepositeCalcInfo.CalculationFormula)
            {
                case DepositCalculationFormulaEnumView.SimpleInterest:
                    return CalculateSimpleInterestDepositePerMonth;
                default:
                    return CalculateCompoundInterestDepositePerMonth;
            }
        }

        private (decimal MonthSum, float Percents) CalculateSimpleInterestDepositePerMonth(int monthNumber, CalculateDepositView reqDepositCalcInfo)
        {
            // An = A(1 + (n / 12) * (P / 100))
            float percentsDevidedBy1200 = (float) reqDepositCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositCalcInfo.DepositSum * (decimal)(1.0f + monthNumber * percentsDevidedBy1200);
            float percents = (float) decimal.Round((decimal)(monthNumber / 12.0f) * reqDepositCalcInfo.Percents, 2);
            return (monthSum, percents);
        }

        private (decimal MonthSum, float Percents) CalculateCompoundInterestDepositePerMonth(int monthNumber, CalculateDepositView reqDepositCalcInfo)
        {
            // An = A(1 + (P / 1200)) ^ (n)
            float percentsDevidedBy1200 = (float) reqDepositCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositCalcInfo.DepositSum * (decimal)Math.Pow(1.0 + percentsDevidedBy1200, monthNumber);
            float percents = (float)decimal.Round(((monthSum - reqDepositCalcInfo.DepositSum) / reqDepositCalcInfo.DepositSum) * 100.0m, 2);
            return (monthSum, percents);
        }
    }
}
