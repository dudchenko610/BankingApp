using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.MainPage;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BankingApp.UI.UnitTests.Components
{
    public class CircleLoaderTests : TestContext
    {
        private readonly Mock<ILoaderService> _loaderServiceMock;

        public CircleLoaderTests()
        {
            _loaderServiceMock = new Mock<ILoaderService>();
            _loaderServiceMock.Setup(x => x.SwitchOn());
            _loaderServiceMock.Setup(x => x.SwitchOff());
            Services.AddSingleton(_loaderServiceMock.Object);

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            navWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns("");
            Services.AddSingleton(navWrapperMock.Object);
        }

        [Fact]
        public void CircleLoader_SwitchOnLoader_LoaderComponentAppersInRenderingTree()
        {
            var mainPage = RenderComponent<MainPage>();

            
        }
    }
}
