using AutoMapper;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.History;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Enums;
using BankingApp.ViewModels.Banking.Calculate;

namespace BankingApp.BusinessLogicLayer.Mapper
{
    public class DepositeHistoryProfile : Profile
    {
        public DepositeHistoryProfile()
        {
            CreateMap<RequestCalculateDepositeBankingView, DepositeHistory>();

            CreateMap<DepositeHistory, ResponseCalculationHistoryBankingViewItem>()
                .ForMember(i => i.DepositePerMonthInfo, model => model.MapFrom(c => c.DepositeHistoryItems))
                .ForMember(i => i.CalculationFormula,
                    model => model.MapFrom(c => ((DepositeCalculationFormulaEnumView)c.CalculationFormula).GetDisplayValue()));
        }
    }
}
