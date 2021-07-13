using BankingApp.UI.Components.DepositeHistoryList;
using BankingApp.ViewModels.Banking.History;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Bunit;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositeHistoryListTests : TestContext
    {
        [Fact]
        public void DepositeHistoryList_PassValidListData_ComponentContainsAsMuchDivWithSpecifiedClassElementsAsListDataCount()
        {
            var testDepositeHistoryList = GetTestDepositesHistoryList();
            var depositeHistoryList = RenderComponent<DepositeHistoryList>(parameters => parameters
                .Add(p => p.DepositesHistoryList, testDepositeHistoryList)
            );

            depositeHistoryList.FindAll("div[class=card-header]").Count.Should().Be(testDepositeHistoryList.Count);
        }

        [Fact]
        public void DepositeHistoryList_UserClicksHistoryItem_EventTriggers()
        {
            int depositeHistoryId = -1;
            var testDepositeHistoryList = GetTestDepositesHistoryList();

            var depositeHistoryList = RenderComponent<DepositeHistoryList>(parameters => parameters
                .Add(p => p.DepositesHistoryList, testDepositeHistoryList)
                .Add(p => p.OnDepositeHistoryItemClicked,
                    (id) => { depositeHistoryId = id; }
                )
            );

            depositeHistoryList.Find("button.btn").Click();
            depositeHistoryId.Should().NotBe(-1);
        }

        private IList<DepositeInfoResponseCalculationHistoryBankingViewItem> GetTestDepositesHistoryList()
        {
            return new List<DepositeInfoResponseCalculationHistoryBankingViewItem>
            {
                new DepositeInfoResponseCalculationHistoryBankingViewItem { Id = 1 },
                new DepositeInfoResponseCalculationHistoryBankingViewItem { Id = 2 },
                new DepositeInfoResponseCalculationHistoryBankingViewItem { Id = 3 }
            };
        }
    }
}
