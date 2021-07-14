using BankingApp.UI.Components.DepositList;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Bunit;
using BankingApp.ViewModels.Banking.Deposit;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositListTests : TestContext
    {
        [Fact]
        public void DepositList_PassValidListData_ComponentContainsAsMuchDivWithSpecifiedClassElementsAsListDataCount()
        {
            var depositGetAllDepositViewItems = GetValidDepositGetAllDepositViewItemList();
            var depositeHistoryList = RenderComponent<DepositList>(parameters => parameters
                .Add(p => p.DepositViewList, depositGetAllDepositViewItems)
            );

            depositeHistoryList.FindAll("div[class=card-header]").Count.Should().Be(depositGetAllDepositViewItems.Count);
        }

        [Fact]
        public void DepositList_UserClicksHistoryItem_EventTriggers()
        {
            int depositeHistoryId = -1;
            var depositGetAllDepositViewItems = GetValidDepositGetAllDepositViewItemList();

            var depositeHistoryList = RenderComponent<DepositList>(parameters => parameters
                .Add(p => p.DepositViewList, depositGetAllDepositViewItems)
                .Add(p => p.OnDepositItemClicked,
                    (id) => { depositeHistoryId = id; }
                )
            );

            depositeHistoryList.Find("button.btn").Click();
            depositeHistoryId.Should().NotBe(-1);
        }

        private IList<DepositGetAllDepositViewItem> GetValidDepositGetAllDepositViewItemList()
        {
            return new List<DepositGetAllDepositViewItem>
            {
                new DepositGetAllDepositViewItem { Id = 1 },
                new DepositGetAllDepositViewItem { Id = 2 },
                new DepositGetAllDepositViewItem { Id = 3 }
            };
        }
    }
}
