using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.ViewModels.Enums;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class DepositService : IDepositService
    {
        private readonly IMapper _mapper;
        private readonly IDepositRepository _depositRepository;
        private readonly IUserService _userService;

        private delegate (decimal MonthSum, float Percents) CalculationFormula(int monthNumber, CalculateDepositView reqDepositeCalcInfo);

        public DepositService(IMapper mapper, IDepositRepository depositeHistoryRepository, IUserService userService)
        {
            _mapper = mapper;
            _depositRepository = depositeHistoryRepository;
            _userService = userService;
        }

        public async Task<int> CalculateAsync(CalculateDepositView calculateDepositView)
        {
            var depositModel = _mapper.Map<CalculateDepositView, Deposit>(calculateDepositView);
            
            depositModel.CalсulationDateTime = System.DateTime.Now;
            CalculationFormula calculationFormula = GetCalculationFormula(calculateDepositView);

            for (int i = 1; i <= calculateDepositView.MonthsCount; i++)
            {
                var res = calculationFormula(i, calculateDepositView);
                depositModel.MonthlyPayments.Add(new MonthlyPayment
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(res.MonthSum, 2),
                    Percents = res.Percents
                });
            }
            depositModel.UserId = _userService.GetSignedInUserId();

            int savedId = await _depositRepository.AddAsync(depositModel);
            return savedId;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositId)
        {
            var depositWithItems = await _depositRepository.GetDepositWithItemsByIdAsync(depositId);
            if (depositWithItems == null)
            { 
                throw new Exception(Constants.Errors.Deposit.IncorrectDepositeHistoryId);
            }

            int userId = _userService.GetSignedInUserId();
            if (depositWithItems.UserId != userId)
            {
                throw new Exception(Constants.Errors.Deposit.DepositDoesNotBelongsToYou);
            }

            var depositWithItemsView = _mapper.Map<Deposit, GetByIdDepositView>(depositWithItems);
            return depositWithItemsView;
        }

        private CalculationFormula GetCalculationFormula(CalculateDepositView calculateDepositView)
        {
            switch (calculateDepositView.CalculationFormula)
            {
                case DepositCalculationFormulaEnumView.SimpleInterest:
                    return CalculateSimpleInterestDepositePerMonth;
                default:
                    return CalculateCompoundInterestDepositePerMonth;
            }
        }

        private (decimal MonthSum, float Percents) CalculateSimpleInterestDepositePerMonth(int monthNumber, CalculateDepositView calculateDepositView)
        {
            // An = A(1 + (n / 12) * (P / 100))
            float percentsDevidedBy1200 = (float) calculateDepositView.Percents / 1200.0f;
            decimal monthSum = calculateDepositView.DepositSum * (decimal)(1.0f + monthNumber * percentsDevidedBy1200);
            float percents = (float) decimal.Round((decimal)(monthNumber / 12.0f) * calculateDepositView.Percents, 2);
            return (monthSum, percents);
        }

        private (decimal MonthSum, float Percents) CalculateCompoundInterestDepositePerMonth(int monthNumber, CalculateDepositView calculateDepositView)
        {
            // An = A(1 + (P / 1200)) ^ (n)
            float percentsDevidedBy1200 = (float) calculateDepositView.Percents / 1200.0f;
            decimal monthSum = calculateDepositView.DepositSum * (decimal)Math.Pow(1.0 + percentsDevidedBy1200, monthNumber);
            float percents = (float)decimal.Round(((monthSum - calculateDepositView.DepositSum) / calculateDepositView.DepositSum) * 100.0m, 2);
            return (monthSum, percents);
        }

        public async Task<ViewModels.ViewModels.Pagination.PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new Exception(Constants.Errors.Page.IncorrectPageNumberFormat);

            if (pageSize < 1)
                throw new Exception(Constants.Errors.Page.IncorrectPageSizeFormat);

            int userId = _userService.GetSignedInUserId();

            DataAccessLayer.Models.PagedDataView<Deposit> depositsAndTotalCount
                = await _depositRepository.GetAllAsync((pageNumber - 1) * pageSize, pageSize, userId);

            var pagedResponse = new ViewModels.ViewModels.Pagination.PagedDataView<DepositGetAllDepositViewItem>
            {
                Items = _mapper.Map<IList<Deposit>, IList<DepositGetAllDepositViewItem>>(depositsAndTotalCount.Items),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = depositsAndTotalCount.TotalCount
            };

            return pagedResponse;
        }
    }
}
