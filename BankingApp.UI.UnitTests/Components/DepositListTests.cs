using BankingApp.UI.Components.DepositList;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Bunit;
using System.Linq;
using BankingApp.ViewModels.ViewModels.Deposit;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositListTests : TestContext
    {
        [Fact]
        public void DepositList_PassValidListData_ComponentContainsAsMuchDivWithSpecifiedClassElementsAsListDataCount()
        {
            var depositGetAllDepositViewItems = GetValidDepositGetAllDepositViewItemList();
            var depositHistoryList = RenderComponent<DepositList>(parameters => parameters
                .Add(p => p.DepositViewList, depositGetAllDepositViewItems)
            );

            depositHistoryList.FindAll("div[class=card-header]").Count.Should().Be(depositGetAllDepositViewItems.Count);
        }

        [Fact]
        public void DepositList_PassValidListData_ComponentRendersAllItemsFieldsInLabelTags()
        {
            const int LabelCount = 4;

            var depositGetAllDepositViewItems = GetValidDepositGetAllDepositViewItemList();
            var depositHistoryList = RenderComponent<DepositList>(parameters => parameters
                .Add(p => p.DepositViewList, depositGetAllDepositViewItems)
            );

            var listOfLabelTexts = depositHistoryList.FindAll("label").Select(x => x.TextContent).ToList();

            for (int i = 0; i < depositGetAllDepositViewItems.Count; i ++)
            {
                listOfLabelTexts[i * LabelCount + 0].Should().Contain(depositGetAllDepositViewItems[i].CalculationFormula);
                listOfLabelTexts[i * LabelCount + 1].Should().Contain(depositGetAllDepositViewItems[i].DepositSum.ToString());
                listOfLabelTexts[i * LabelCount + 2].Should().Contain(depositGetAllDepositViewItems[i].MonthsCount.ToString());
                listOfLabelTexts[i * LabelCount + 3].Should().Contain(depositGetAllDepositViewItems[i].Percents.ToString());
            }
        }

        [Fact]
        public void DepositList_UserClicksHistoryItem_EventTriggers()
        {
            int depositHistoryId = -1;
            var depositGetAllDepositViewItems = GetValidDepositGetAllDepositViewItemList();

            var depositHistoryList = RenderComponent<DepositList>(parameters => parameters
                .Add(p => p.DepositViewList, depositGetAllDepositViewItems)
                .Add(p => p.OnDepositItemClicked,
                    (id) => { depositHistoryId = id; }
                )
            );

            depositHistoryList.Find("button.btn").Click();
            depositHistoryId.Should().NotBe(-1);
        }

        private IList<DepositGetAllDepositViewItem> GetValidDepositGetAllDepositViewItemList()
        {
            return new List<DepositGetAllDepositViewItem>
            {
                new DepositGetAllDepositViewItem 
                { 
                    Id = 1,
                    CalculationFormula = "smth1",
                    DepositSum = 100,
                    MonthsCount = 12,
                    Percents = 4,
                    CalсulationDateTime = System.DateTime.MaxValue
                },
                new DepositGetAllDepositViewItem 
                {
                    Id = 2,
                    CalculationFormula = "smth2",
                    DepositSum = 200,
                    MonthsCount = 6,
                    Percents = 5,
                    CalсulationDateTime = System.DateTime.MinValue
                },
                new DepositGetAllDepositViewItem 
                { 
                    Id = 3,
                    CalculationFormula = "smth3",
                    DepositSum = 300,
                    MonthsCount = 2,
                    Percents = 2,
                    CalсulationDateTime = System.DateTime.MinValue
                }
            };
        }
    }
}
