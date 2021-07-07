using BankingApp.UI.Components.DepositeHistoryList;
using BankingApp.ViewModels.Banking.History;
using Bunit;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositeHistoryListTests : TestContext
    {
        private IList<ResponseCalculationHistoryBankingViewItem> _depositesHistoryList;

        public DepositeHistoryListTests()
        {
            _depositesHistoryList = new List<ResponseCalculationHistoryBankingViewItem>
            {
                new ResponseCalculationHistoryBankingViewItem { Id = 1 },
                new ResponseCalculationHistoryBankingViewItem { Id = 2 },
                new ResponseCalculationHistoryBankingViewItem { Id = 3 }
            };
        }

        [Fact]
        public void DepositeHistoryList_PassValidListData_ComponentContainsAsMuchDivWithSpecifiedClassElementsAsListDataCount()
        {
            var depositeHistoryList = RenderComponent<DepositeHistoryList>(parameters => parameters
                .Add(p => p.DepositesHistoryList, _depositesHistoryList)
            );

            depositeHistoryList.FindAll("div[class=card-header]").Count.Should().Be(_depositesHistoryList.Count);
        }

        [Fact]
        public void DepositeHistoryList_UserClicksHistoryItem_EventTriggers()
        {
            int depositeHistoryId = -1;

            var depositeHistoryList = RenderComponent<DepositeHistoryList>(parameters => parameters
                .Add(p => p.DepositesHistoryList, _depositesHistoryList)
                .Add(p => p.OnDepositeHistoryItemClicked, 
                    (id) => { depositeHistoryId = id; }
                )
            );

            depositeHistoryList.Find("button.btn").Click();
            depositeHistoryId.Should().NotBe(-1);
        }
    }
}
