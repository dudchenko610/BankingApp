using BusinessLogicLayer.Interfaces;
using ViewModels.Banking;

namespace BusinessLogicLayer.Services
{
    public class BankingService : IBankingService
    {
        public ResponseCalculateDepositeBankingView CalculateDeposite(RequestCalculateDepositeBankingView reqDepositeCalcInfo)
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
    }
}
