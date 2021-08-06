using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Shared.Header;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;
using BankingApp.ViewModels.ViewModels.Authentication;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.UI.UnitTests.Shared
{
    public class HeaderTests : TestContext
    {
        private readonly Mock<INavigationWrapper> _navWrapperMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<ILoaderService> _loaderServiceMock;
        public HeaderTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationServiceMock.SetupGet(x => x.TokensView).Returns(GetValidTokensView());

            _loaderServiceMock = new Mock<ILoaderService>();
            _loaderServiceMock.Setup(x => x.SwitchOn());
            _loaderServiceMock.Setup(x => x.SwitchOff());

            _navWrapperMock = new Mock<INavigationWrapper>();
            _navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            _navWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns("");

            Services.AddSingleton(_authenticationServiceMock.Object);
            Services.AddSingleton(_loaderServiceMock.Object);
            Services.AddSingleton(_navWrapperMock.Object);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserClicksUrlLink_ExpectedMarkupRendered()
        {
            Services.AddSingleton(_authenticationServiceMock.Object);
            Services.AddSingleton(_loaderServiceMock.Object);
            Services.AddSingleton(_navWrapperMock.Object);

            var headerComponent = RenderComponent<Header>();
            headerComponent.Find("a").Click();
            headerComponent.Find("li").ClassList.Should().Contain("active");

            _navWrapperMock.Verify(mock => mock.NavigateTo(It.IsAny<string>(), false), Times.AtLeastOnce);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserIsLoggedAsClient_ExpectedMarkupRendered()
        {
            var validTokensView = GetValidTokensView();

            _authenticationServiceMock.SetupGet(x => x.TokensView).Returns(GetValidTokensView());

            var mainLayoutComp = RenderComponent<Header>();
            var headerLinks = mainLayoutComp.FindAll("a").Select(x => x.TextContent);

            headerLinks.Should().BeEquivalentTo(GetClientHeaderLinks());
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserIsNotLogged_ExpectedMarkupRendered()
        {
            _authenticationServiceMock.SetupGet(x => x.TokensView).Returns((TokensView) null);

            var mainLayoutComp = RenderComponent<Header>();
            var headerLinks = mainLayoutComp.FindAll("a").Select(x => x.TextContent);

            headerLinks.Should().BeEquivalentTo(GetUnauthorizedHeaderLinks());
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserIsLoggedAsAdmin_ExpectedMarkupRendered()
        {
            var validTokensView = GetValidTokensView();

            _authenticationServiceMock.SetupGet(x => x.TokensView).Returns(validTokensView);
            _authenticationServiceMock.SetupGet(x => x.IsAdmin).Returns(true);

            var mainLayoutComp = RenderComponent<Header>();
            var headerLinks = mainLayoutComp.FindAll("a").Select(x => x.TextContent);

            headerLinks.Should().BeEquivalentTo(GetAdminHeaderLinks());
        }

        private TokensView GetValidTokensView()
        {
            return new TokensView
            {
                AccessToken = "this_is_my_token"
            };
        }

        private IList<string> GetClientHeaderLinks()
        { 
            return new List<string>()
            {
                "MainPage",
                "History",
                "Logout"
            };
        }

        private IList<string> GetUnauthorizedHeaderLinks()
        {
            return new List<string>()
            {
                "SignIn",
                "SignUp"
            };
        }

        private IList<string> GetAdminHeaderLinks()
        {
            return new List<string>()
            {
                "MainPage",
                "History",
                "Users",
                "Logout"
            };
        }
    }
}
