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
        public void LogoutPage_UserSubmitsValidData_CallbacksTriggerAndReturnValidData()
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
