using BankingApp.UI.Components.PaginationSwitcher;
using Bunit;
using Xunit;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Components
{
    public class PaginationSwitcherTests : TestContext
    {
        private const int PageCount = 10;
        private const int CurrentPage = 5;

        [Fact]
        public void PaginationSwitcher_UserClicksPagelink_CallbackReturnsClickedPageNumber()
        {
            int pageNumber = -1;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCount)
                .Add(p => p.CurrentPage, CurrentPage)
                .Add(p => p.OnPageClick,
                    (pNum) => { pageNumber = pNum; })
            );

            var aTagList = paginationSwitcher.FindAll("a");
            aTagList[CurrentPage % 3].Click();

            pageNumber.Should().Be(CurrentPage);
        }

        [Fact]
        public void PaginationSwitcher_UserClicksPagelink_PageLinkGetsActiveState()
        {
            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCount)
                .Add(p => p.CurrentPage, CurrentPage)
            );

            var aTagList = paginationSwitcher.FindAll("a");
            aTagList[CurrentPage % 3].Click();

            var liActiveTag = paginationSwitcher.Find("li.active");
            liActiveTag.FirstChild.TextContent.Should().Be(CurrentPage.ToString());
        }
    }
}
