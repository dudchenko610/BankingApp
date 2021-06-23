using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Shared.ViewModels.Banking
{
    public class DepositeInputData
    {
      //  [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = Constants.Errors.Banking.INCORRECT_PRICE_FORMAT)]
        public decimal DepositSum { get; set; }

     //   [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Banking.INCORRECT_MONTH_NUMBER)]
        public int Months { get; set; }

      //  [Range(1, 100, ErrorMessage = Constants.Errors.Banking.INCORRECT_PERECENT_NUMBER)]
        public int Percents { get; set; }
    }
}
