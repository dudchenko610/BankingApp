using BankingApp.UI.Components.MonthlyPaymentList;
using Bunit;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using BankingApp.ViewModels.ViewModels.Deposit;

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

        [Fact]
        public void MonthlyPaymentList_PassValidListData_ComponentRendersAllItemsFieldsInLabelTags()
        {
            const int LabelCount = 3;

            var monthlyPaymentGetByIdDepositViewItems = GetValidMonthlyPaymentGetByIdDepositViewItemList();
            var monthlyPaymentList = RenderComponent<MonthlyPaymentList>(parameters => parameters
                .Add(p => p.MonthlyPaymentViewList, monthlyPaymentGetByIdDepositViewItems)
            );

            var listOfLabelTexts = monthlyPaymentList.FindAll("label").Select(x => x.TextContent).ToList();

            for (int i = 0; i < monthlyPaymentGetByIdDepositViewItems.Count; i++)
            {
                listOfLabelTexts[i * LabelCount + 0].Should().Contain(monthlyPaymentGetByIdDepositViewItems[i].MonthNumber.ToString());
                listOfLabelTexts[i * LabelCount + 1].Should().Contain(monthlyPaymentGetByIdDepositViewItems[i].TotalMonthSum.ToString());
                listOfLabelTexts[i * LabelCount + 2].Should().Contain(monthlyPaymentGetByIdDepositViewItems[i].Percents.ToString());
            }
        }

        private IList<MonthlyPaymentGetByIdDepositViewItem> GetValidMonthlyPaymentGetByIdDepositViewItemList()
        { 
            return new List<MonthlyPaymentGetByIdDepositViewItem>
            {
                new MonthlyPaymentGetByIdDepositViewItem 
                {
                    MonthNumber = 1,
                    Percents = 1.4f,
                    TotalMonthSum = 100
                },
                new MonthlyPaymentGetByIdDepositViewItem
                {
                    MonthNumber = 2,
                    Percents = 2.5f,
                    TotalMonthSum = 102
                },
                new MonthlyPaymentGetByIdDepositViewItem 
                {
                    MonthNumber = 3,
                    Percents = 4.5f,
                    TotalMonthSum = 106
                }
            };
        }
    }
}
