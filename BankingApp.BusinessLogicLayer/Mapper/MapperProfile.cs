using AutoMapper;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Enums;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Banking.Authentication;

namespace BankingApp.BusinessLogicLayer.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CalculateDepositView, Deposit>();

            CreateMap<Deposit, DepositGetAllDepositViewItem>()
                .ForMember(i => i.CalculationFormula,
                    model => model.MapFrom(c => ((DepositCalculationFormulaEnumView)c.CalculationFormula).GetDisplayValue()));

            CreateMap<Deposit, GetByIdDepositView>()
                .ForMember(i => i.MonthlyPaymentItems, model => model.MapFrom(c => c.MonthlyPayments))
                .ForMember(i => i.CalculationFormula,
                    model => model.MapFrom(c => ((DepositCalculationFormulaEnumView)c.CalculationFormula).GetDisplayValue()));
       
            CreateMap<MonthlyPayment, MonthlyPaymentGetByIdDepositViewItem>();

            CreateMap<SignUpAuthenticationView, User>()
                .ForMember(x => x.UserName, model => model.MapFrom(c => c.Nickname));
        }
    }
}
