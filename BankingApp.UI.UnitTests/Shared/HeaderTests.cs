using BankingApp.UI.Shared.Header;
using Bunit;
using Xunit;

namespace BankingApp.UI.UnitTests.Shared
{
    public class HeaderTests : TestContext
    {
        [Fact]
        public void Header_UserClicksUrlLink_LinkGetsActiveState()
        {
            var mainLayoutComp = RenderComponent<Header>();

            Assert.Contains(mainLayoutComp.Find("li").ClassList, i => i != "active");
            mainLayoutComp.Find("a").Click();
            Assert.Contains(mainLayoutComp.Find("li").ClassList, i => i == "active");
        }
    }
}
