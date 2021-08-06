using BankingApp.UI.Components.PaginationSwitcher;
using Bunit;
using Xunit;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Components
{
    public class PaginationSwitcherTests : TestContext
    {
        private const string DisabledClass = "disabled";
        private const int PageCount = 10;
        private const int CurrentPage = 5;

        [Fact]
        public void WhenTheCallbackIsTriggered_UserClicksPagelink_OnPageClickInvoked()
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
        public void WhenTheComponentIsRendered_UserClicksPagelink_ExpectedMarkupRendered()
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

        [Fact]
        public void WhenTheComponentIsRendered_PageCountLessThanFourElements_ExpectedMarkupRendered()
        {
            const int PageCountLessThanFour = 3;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCountLessThanFour)
                .Add(p => p.CurrentPage, CurrentPage)
            );

            var liNext = paginationSwitcher.Find("a[aria-label=Next]").ParentElement;
            var liPrevious = paginationSwitcher.Find("a[aria-label=Previous]").ParentElement;

            liNext.ClassList.Should().Contain(DisabledClass);
            liPrevious.ClassList.Should().Contain(DisabledClass);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserScrollsToRightMaximally_ExpectedMarkupRendered()
        {
            const int CurrentPageMaximallyRight = PageCount;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCount)
                .Add(p => p.CurrentPage, CurrentPageMaximallyRight)
            );

            var liNext = paginationSwitcher.Find("a[aria-label=Next]").ParentElement;
            var liPrevious = paginationSwitcher.Find("a[aria-label=Previous]").ParentElement;

            liNext.ClassList.Should().Contain(DisabledClass);
            liPrevious.ClassList.Should().NotContain(DisabledClass);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserScrollsToLeftMaximally_ExpectedMarkupRendered()
        {
            const int CurrentPageMaximallyLeft = 1;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCount)
                .Add(p => p.CurrentPage, CurrentPageMaximallyLeft)
            );

            var liNext = paginationSwitcher.Find("a[aria-label=Next]").ParentElement;
            var liPrevious = paginationSwitcher.Find("a[aria-label=Previous]").ParentElement;

            liNext.ClassList.Should().NotContain(DisabledClass);
            liPrevious.ClassList.Should().Contain(DisabledClass);
        }

        [Fact]
        public void WhenTheComponentIsRendered_PageCountParameterLessThanOne_ExpectedMarkupRendered()
        {
            const int PageCountLessThanOne = 0;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCountLessThanOne)
                .Add(p => p.CurrentPage, CurrentPage)
            );

            paginationSwitcher.Instance.PageCount.Should().Be(1);
        }

        [Fact]
        public void WhenTheComponentIsRendered_CurrentPageParameterLessThanOne_ExpectedMarkupRendered()
        {
            const int CurrentPageLessThanOne = 0;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCount)
                .Add(p => p.CurrentPage, CurrentPageLessThanOne)
            );

            paginationSwitcher.Instance.CurrentPage.Should().Be(1);
        }

        [Fact]
        public void WhenTheComponentIsRendered_CurrentPageParameterBiggerThanPageCount_ExpectedMarkupRendered()
        {
            const int CurrentPageBiggerThanPageCount = PageCount + 1;

            var paginationSwitcher = RenderComponent<PaginationSwitcher>(parameters => parameters
                .Add(p => p.PageCount, PageCount)
                .Add(p => p.CurrentPage, CurrentPageBiggerThanPageCount)
            );

            paginationSwitcher.Instance.CurrentPage.Should().Be(1);
        }
    }
}
