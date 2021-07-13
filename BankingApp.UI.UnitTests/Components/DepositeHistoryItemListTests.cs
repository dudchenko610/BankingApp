using BankingApp.UI.Components.DepositeHistoryItemList;
using Bunit;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using BankingApp.ViewModels.Banking.History;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositeHistoryItemListTests : TestContext
    {
        [Fact]
        public void DepositeHistoryItemList_PassValidListData_ComponentContainsAsMuchLiElementsAsListDataCount()
        {
            var perMonthInfos = new List<MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem>
            {
                new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem { },
                new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem { },
                new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem { }
            };

            var depositeHistoryItemList = RenderComponent<DepositeHistoryItemList>(parameters => parameters
                .Add(p => p.PerMonthInfo, perMonthInfos)
            );

            depositeHistoryItemList.FindAll("li[class=list-group-item]").Count.Should().Be(perMonthInfos.Count);
        }
    }
}
