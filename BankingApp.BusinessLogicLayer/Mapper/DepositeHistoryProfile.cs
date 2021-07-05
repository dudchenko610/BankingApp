using AutoMapper;
using BankingApp.DataAccessLayer.Entities;
using BankingApp.ViewModels.Banking;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.History;

namespace BankingApp.BusinessLogicLayer.Mapper
{
    public class DepositeHistoryProfile : Profile
    {
        public DepositeHistoryProfile()
        {
            CreateMap<RequestCalculateDepositeBankingView, DepositeHistory>()
                .ForMember(i => i.CalculationFormula, model => model.MapFrom(c => c.CalculationFormula.GetDisplayValue()));

            CreateMap<DepositeHistory, ResponseCalculationHistoryBankingViewItem>()
                .ForMember(i => i.DepositePerMonthInfo, model => model.MapFrom(c => c.DepositeHistoryItems));
        }
    }
}
