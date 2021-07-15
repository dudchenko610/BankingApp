using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using Bunit;
using FluentAssertions;
using Xunit;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class LoaderServiceTests : TestContext
    {
        private ILoaderService _loaderService;

        public LoaderServiceTests()
        {
            _loaderService = new LoaderService();
        }

        [Fact]
        public void SwitchOn_SwitchOnLoader_CircleLoaderComponentAppearsInRendringTree()
        {
            bool switched = false;
            _loaderService.OnLoaderSwitch += (isSwitched) => { switched = isSwitched; };

            _loaderService.SwitchOn();
            switched.Should().BeTrue();
        }

        [Fact]
        public void SwitchOff_SwitchOffLoader_CircleLoaderComponentDisappearsFromRendringTree()
        {
            bool switched = true;
            _loaderService.OnLoaderSwitch += (isSwitched) => { switched = isSwitched; };

            _loaderService.SwitchOff();
            switched.Should().BeFalse();
        }
    }
}
