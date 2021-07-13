using AutoMapper;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Enums;

namespace BankingApp.BusinessLogicLayer.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CalculateDepositeBankingView, DepositeHistory>();

            CreateMap<DepositeHistory, DepositeInfoResponseCalculationHistoryBankingViewItem>()
                .ForMember(i => i.CalculationFormula,
                    model => model.MapFrom(c => ((DepositeCalculationFormulaEnumView)c.CalculationFormula).GetDisplayValue()));

            CreateMap<DepositeHistory, ResponseCalculationHistoryDetailsBankingView>()
                .ForMember(i => i.DepositePerMonthInfo, model => model.MapFrom(c => c.DepositeHistoryItems))
                .ForMember(i => i.CalculationFormula,
                    model => model.MapFrom(c => ((DepositeCalculationFormulaEnumView)c.CalculationFormula).GetDisplayValue()));
       
            CreateMap<DepositeHistoryItem, MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem>();
        }
    }
}
