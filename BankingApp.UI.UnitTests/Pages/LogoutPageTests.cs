using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.LogoutPage;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Pages
{
    public class LogoutPageTests : TestContext
    {
        [Fact]
        public void WhenTheComponentIsRendered_UserSubmitsValidData_LogoutAsyncInvoked()
        {
            bool logoutWasCalled = false;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.LogoutAsync()).Callback(() => { logoutWasCalled = true; });

            Services.AddSingleton(authenticationServiceMock.Object);
            var forgotPasswordForm = RenderComponent<LogoutPage>();

            logoutWasCalled.Should().BeTrue();
        }
    }
}
