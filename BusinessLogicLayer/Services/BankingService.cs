using BusinessLogicLayer.Facade;
using Shared.ViewModels.Banking;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Services
{
    public class BankingService : IBankingService
    {
        public DepositeOutputData CalculateDeposite(DepositeInputData inData)
        {
            var outData = new DepositeOutputData();
            // An = A(1 + (n / 12) * (P / 100))

            float percDev12 = inData.Percents / 1200.0f;

            for (int i = 1; i <= inData.Months; i++)
            {
                float monthSum = inData.DepositSum * (1.0f + i * percDev12);

                outData.PerMonthInfos.Add(new DepositeMonthInfo
                { 
                    MonthNumber = i,
                    TotalMonthSum = decimal.Round((decimal)monthSum, 2),
                    Percents = (int) ((i / 12.0f) * inData.Percents)
                });
            }

            return outData;
        }
    }
}
