using BankingApp.UI.Components.DepositeHistoryItemList;
using Bunit;
using Xunit;
using FluentAssertions;
using BankingApp.ViewModels.Banking;
using System.Collections.Generic;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositeHistoryItemListTests : TestContext
    {
        [Fact]
        public void DepositeHistoryItemList_PassValidListData_ComponentContainsAsMuchLiElementsAsListDataCount()
        {
            var perMonthInfos = new List<ResponseCalculateDepositeBankingViewItem>
            {
                new ResponseCalculateDepositeBankingViewItem { },
                new ResponseCalculateDepositeBankingViewItem { },
                new ResponseCalculateDepositeBankingViewItem { }
            };

            var depositeHistoryItemList = RenderComponent<DepositeHistoryItemList>(parameters => parameters
                .Add(p => p.PerMonthInfos, perMonthInfos)
            );

            depositeHistoryItemList.FindAll("li[class=list-group-item]").Count.Should().Be(perMonthInfos.Count);
        }
    }
}
