using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using BankingApp.Entities.Entities;
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
    }
}
