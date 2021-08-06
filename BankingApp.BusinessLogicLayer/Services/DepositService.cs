using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.ViewModels.Enums;
using BankingApp.ViewModels.ViewModels.Deposit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    /// <summary>
    /// Allows the user to make deposit calculations and get the calculation history.
    /// </summary>
    public class DepositService : IDepositService
    {
        private readonly IMapper _mapper;
        private readonly IDepositRepository _depositRepository;
        private readonly IUserService _userService;

        private delegate (decimal MonthSum, float Percents) CalculationFormula(int monthNumber, CalculateDepositView reqDepositeCalcInfo);

        /// <summary>
        /// Creates instance of <see cref="DepositService"/>.
        /// </summary>
        /// <param name="mapper">Allows to map models.</param>
        /// <param name="depositRepository">Allows manipulate with deposits in storage.</param>
        /// <param name="userService">Allows to provide operations with users.</param>
        public DepositService(IMapper mapper, IDepositRepository depositRepository, IUserService userService)
        {
            _mapper = mapper;
            _depositRepository = depositRepository;
            _userService = userService;
        }

        /// <summary>
        /// Calculates deposit by passed input data.
        /// </summary>
        /// <param name="calculateDepositView">Contains input deposit data.</param>
        /// <returns>Id of saved deposit.</returns>
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

        /// <summary>
        /// Allows to get information about deposit with monthly payments information.
        /// </summary>
        /// <param name="depositId">Id of deposit in storage.</param>
        /// <exception cref="Exception">If there is no deposit with such id or fetched deposit belong to another user.</exception>
        /// <returns>View model representing deposit.</returns>
        public async Task<GetByIdDepositView> GetByIdAsync(int depositId)
        {
            var depositWithItems = await _depositRepository.GetDepositWithItemsByIdAsync(depositId);

            if (depositWithItems == null)
            { 
                throw new Exception(Constants.Errors.Deposit.IncorrectDepositHistoryId);
            }

            int userId = _userService.GetSignedInUserId();

            if (depositWithItems.UserId != userId)
            {
                throw new Exception(Constants.Errors.Deposit.DepositDoesNotExistsOrYouHaveNoAccess);
            }

            var depositWithItemsView = _mapper.Map<Deposit, GetByIdDepositView>(depositWithItems);

            return depositWithItemsView;
        }

        /// <summary>
        /// Allows getting page of user's deposits.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <exception cref="Exception">If some of the parameters or both are invalid</exception>
        /// <returns>View model with data about all deposits in storage and deposits list for specified page.</returns>
        public async Task<ViewModels.ViewModels.Pagination.PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new Exception(Constants.Errors.Page.IncorrectPageNumberFormat);
            }

            if (pageSize < 1)
            {
                throw new Exception(Constants.Errors.Page.IncorrectPageSizeFormat);
            }

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

            PaginationModel<Deposit> depositsAndTotalCount
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
