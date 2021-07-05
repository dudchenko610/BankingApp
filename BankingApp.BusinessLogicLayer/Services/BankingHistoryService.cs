using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Entities;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class BankingHistoryService : IBankingHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IDepositeHistoryRepository _depositeHistoryRepository;

        public BankingHistoryService(IMapper mapper, IDepositeHistoryRepository depositeHistoryRepository)
        {
            _mapper = mapper;
            _depositeHistoryRepository = depositeHistoryRepository;
        }

        public async Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync()
        {
            IList<DepositeHistory> depositesHistoryFromDb = await _depositeHistoryRepository.GetDepositesHistoryWithItemsAsync();

            var response = new ResponseCalculationHistoryBankingView();
            response.DepositesHistory 
                = _mapper.Map<IList<DepositeHistory>, IList<ResponseCalculationHistoryBankingViewItem>>(depositesHistoryFromDb);

            return response;
        }

        public async Task SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo,
            ResponseCalculateDepositeBankingView depositeCalculation)
        {
            var depositeHistory = _mapper.Map<DepositeHistory>(reqDepositeCalcInfo);
            depositeHistory.DepositeHistoryItems 
                = _mapper.Map<IList<ResponseCalculateDepositeBankingViewItem>, IList<DepositeHistoryItem>>(depositeCalculation.PerMonthInfos);

            await _depositeHistoryRepository.AddAsync(depositeHistory);
        }
    }
}
