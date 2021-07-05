using AutoMapper;
using BankingApp.DataAccessLayer.Entities;
using BankingApp.ViewModels.Banking;

namespace BankingApp.BusinessLogicLayer.Mapper
{
    public class DepositeHistoryItemProfile : Profile
    {
        public DepositeHistoryItemProfile()
        {
            CreateMap<ResponseCalculateDepositeBankingViewItem, DepositeHistoryItem>();
            CreateMap<DepositeHistoryItem, ResponseCalculateDepositeBankingViewItem>();
        }
    }
}
