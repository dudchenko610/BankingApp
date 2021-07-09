using BankingApp.UI.Components.CircleLoader;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using BankingApp.UI.Shared;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class LoaderServiceTests : TestContext
    {
        private ILoaderService _loaderService;
        public LoaderServiceTests()
        {
            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns("");
            Services.AddSingleton(navWrapperMock.Object);

            Services.AddSingleton<ILoaderService, LoaderService>();
            _loaderService = Services.GetService<ILoaderService>();
        }

        [Fact]
        public async Task SwitchOn_SwitchOnLoader_CircleLoaderComponentAppearsInRendringTree()
        {
            var depositeForm = RenderComponent<MainLayout>();
            await depositeForm.InvokeAsync(() => { _loaderService.SwitchOn(); });
            depositeForm.FindComponent<CircleLoader>();
        }
    }
}
