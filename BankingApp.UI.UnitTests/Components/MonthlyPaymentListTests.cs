using BankingApp.UI.Components.MonthlyPaymentList;
using Bunit;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using BankingApp.ViewModels.Banking.Deposit;

namespace BankingApp.UI.UnitTests.Components
{
    public class MonthlyPaymentListTests : TestContext
    {
        [Fact]
        public void MonthlyPaymentList_PassValidListData_ComponentContainsAsMuchLiElementsAsListDataCount()
        {
            var monthlyPaymentGetByIdDepositViewItemList = GetValidMonthlyPaymentGetByIdDepositViewItemList();

            var depositeHistoryItemList = RenderComponent<MonthlyPaymentList>(parameters => parameters
                .Add(p => p.MonthlyPaymentViewList, monthlyPaymentGetByIdDepositViewItemList)
            );

            depositeHistoryItemList.FindAll("li[class=list-group-item]").Count.Should().Be(monthlyPaymentGetByIdDepositViewItemList.Count);
        }

        private IList<MonthlyPaymentGetByIdDepositViewItem> GetValidMonthlyPaymentGetByIdDepositViewItemList()
        { 
            return new List<MonthlyPaymentGetByIdDepositViewItem>
            {
                new MonthlyPaymentGetByIdDepositViewItem { },
                new MonthlyPaymentGetByIdDepositViewItem { },
                new MonthlyPaymentGetByIdDepositViewItem { }
            };
        }
    }
}
