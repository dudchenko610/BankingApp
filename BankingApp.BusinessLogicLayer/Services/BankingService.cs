using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Shared.Enums.Banking;
using BankingApp.ViewModels.Banking;
using System;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class BankingService : IBankingService
    {
        private delegate (decimal MonthSum, int Percents) CalculationFormula(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo);

        public ResponseCalculateDepositeBankingView CalculateDeposite(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            var respDepositeInfo = new ResponseCalculateDepositeBankingView();
            CalculationFormula calculationFormula = GetCalculationFormula(reqDepositeCalcInfo);

            for (int i = 1; i <= reqDepositeCalcInfo.MonthsCount; i++)
            {
                var res = calculationFormula(i, reqDepositeCalcInfo);
                respDepositeInfo.PerMonthInfos.Add(new ResponseCalculateDepositeBankingViewItem
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(res.MonthSum, 2),
                    Percents = res.Percents
                });
            }
            return respDepositeInfo;
        }

        private CalculationFormula GetCalculationFormula(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            switch (reqDepositeCalcInfo.CalculationFormula)
            {
                case DepositeCalculationFormula.SimpleInterest:
                    return CalculateSimpleInterestDepositePerMonth;
                default:
                    return CalculateCompoundInterestDepositePerMonth;
            }
        }

        private (decimal MonthSum, int Percents) CalculateSimpleInterestDepositePerMonth(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            // An = A(1 + (n / 12) * (P / 100))
            float percentsDevidedBy1200 = reqDepositeCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal) (1.0f + monthNumber * percentsDevidedBy1200);
            int percents = (int)((monthNumber / 12.0f) * reqDepositeCalcInfo.Percents);
            return (monthSum, percents);
        }

        private (decimal MonthSum, int Percents) CalculateCompoundInterestDepositePerMonth(int monthNumber, RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            // An = A(1 + (P / 1200)) ^ (n)
            float percentsDevidedBy1200 = reqDepositeCalcInfo.Percents / 1200.0f;
            decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal) Math.Pow(1.0 + percentsDevidedBy1200, monthNumber);
            int percents = (int)(((monthSum - reqDepositeCalcInfo.DepositeSum) / reqDepositeCalcInfo.DepositeSum) * 100.0m);
            return (monthSum, percents);
        }
    }
}
