using AutoMapper;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.History;

namespace BankingApp.BusinessLogicLayer.Mapper
{
    public class DepositeHistoryItemProfile : Profile
    {
        public DepositeHistoryItemProfile()
        {
            CreateMap<DepositeHistoryItem, ResponseCalculationHistoryDetailsBankingViewItem>();
        }
    }
}
