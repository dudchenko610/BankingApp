﻿using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Shared.Header;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Shared
{
    public class HeaderTests : TestContext
    {
        private readonly Mock<INavigationWrapper> _navWrapperMock;
        public HeaderTests()
        {
            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn());
            loaderServiceMock.Setup(x => x.SwitchOff());
            Services.AddSingleton(loaderServiceMock.Object);

            _navWrapperMock = new Mock<INavigationWrapper>();
            _navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            _navWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns("");
            Services.AddSingleton(_navWrapperMock.Object);
        }

        [Fact]
        public void Header_UserClicksUrlLink_LinkGetsActiveState()
        {
            var headerComponent = RenderComponent<Header>();

            headerComponent.Find("li").ClassList.Should().NotContain("active");
            headerComponent.Find("a").Click();
            headerComponent.Find("li").ClassList.Should().Contain("active");
        }

        [Fact]
        public void Header_UserClicksUrlLink_AppSwitchesToAnotherRoute()
        { 
            var mainLayoutComp = RenderComponent<Header>();
            mainLayoutComp.Find("a").Click();

            _navWrapperMock.Verify(mock => mock.NavigateTo(It.IsAny<string>(), false), Times.AtLeastOnce);
        }
    }
}
