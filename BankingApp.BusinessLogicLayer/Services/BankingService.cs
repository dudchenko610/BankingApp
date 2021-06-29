using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class BankingService : IBankingService
    {
        public ResponseCalculateDepositeBankingView CalculateDepositeSimpleInterest(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            var respDepositeInfo = new ResponseCalculateDepositeBankingView();

            // An = A(1 + (n / 12) * (P / 100))
            float percentsDevidedBy1200 = reqDepositeCalcInfo.Percents / 1200.0f;
            for (int i = 1; i <= reqDepositeCalcInfo.MonthsCount; i++)
            {
                decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal) (1.0f + i * percentsDevidedBy1200);
                respDepositeInfo.PerMonthInfos.Add(new ResponseCalculateDepositeBankingViewItem
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(monthSum, 2),
                    Percents = (int) ((i / 12.0f) * reqDepositeCalcInfo.Percents)
                });
            }
            return respDepositeInfo;
        }

        public ResponseCalculateDepositeBankingView CalculateDepositeCompoundInterest(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
        {
            var respDepositeInfo = new ResponseCalculateDepositeBankingView();

            // An = A(1 + (P / 100)) * (n / 12)
            float percentsDevidedBy100 = reqDepositeCalcInfo.Percents / 100.0f;
            for (int i = 1; i <= reqDepositeCalcInfo.MonthsCount; i++)
            {
                decimal monthSum = reqDepositeCalcInfo.DepositeSum * (decimal) (1.0f + percentsDevidedBy100) * (i / 12.0m);
                respDepositeInfo.PerMonthInfos.Add(new ResponseCalculateDepositeBankingViewItem
                {
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round(monthSum, 2),
                    Percents = (int) (((monthSum - reqDepositeCalcInfo.DepositeSum) / reqDepositeCalcInfo.DepositeSum) * 100)
                });
            }
            return respDepositeInfo;
        }
    }
}
