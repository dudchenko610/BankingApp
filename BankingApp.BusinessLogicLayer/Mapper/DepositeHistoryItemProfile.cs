using AutoMapper;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Calculate;

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
