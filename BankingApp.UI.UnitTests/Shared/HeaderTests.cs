using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Shared.Header;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BankingApp.UI.UnitTests.Shared
{
    public class HeaderTests : TestContext
    {
        private readonly Mock<FakeNavigationManager> _navManagerMock;
        public HeaderTests()
        {
            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn());
            loaderServiceMock.Setup(x => x.SwitchOff());
            Services.AddSingleton<ILoaderService>(loaderServiceMock.Object);

            _navManagerMock = new Mock<FakeNavigationManager>();
            _navManagerMock.Setup(x => x.NavigateTo()).Verifiable();

            Services.AddSingleton<NavigationManager>(_navManagerMock.Object);
        }

        [Fact]
        public void Header_UserClicksUrlLink_LinkGetsActiveState()
        {
            var mainLayoutComp = RenderComponent<Header>();

            Assert.Contains(mainLayoutComp.Find("li").ClassList, i => i != "active");
            mainLayoutComp.Find("a").Click();
            Assert.Contains(mainLayoutComp.Find("li").ClassList, i => i == "active");
        }
        
        [Fact]
        public void Header_UserClicksUrlLink_AppSwitchesToAnotherRoute()
        { 
            var mainLayoutComp = RenderComponent<Header>();
            mainLayoutComp.Find("a").Click();

            _navManagerMock.Verify(mock => mock.NavigateTo(), Times.AtLeastOnce);
        }
        
    }
}
