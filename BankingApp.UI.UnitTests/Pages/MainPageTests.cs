using BankingApp.UI.Components.CircleLoader;
using BankingApp.UI.Components.DepositeForm;
using BankingApp.UI.Core.Enums;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.MainPage;
using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BankingApp.UI.UnitTests.Pages
{
    public class MainPageTests : TestContext
    {
        private const int DepositeSum = 100;
        private const int MonthsCount = 12;
        private const int Percents = 10;

        public MainPageTests()
        {
            var depositeServiceMock = new Mock<IDepositeService>();
            depositeServiceMock.Setup(repo => repo.CalculateDepositeAsync(null)).Callback( async () => { await Task.Delay(100); });
            depositeServiceMock.Setup(repo => repo.GetCalculationDepositeHistoryAsync()).Callback( async () => { await Task.Delay(100); });

            Services.AddSingleton<IDepositeService>(depositeServiceMock.Object);
        }

        [Fact]
        public void MainPage_UserSubmitsValidData_PageContentReplacesWithLoader() // Deprecated
        {
            var cut = RenderComponent<MainPage>();

            cut.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            cut.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            cut.Find("input[id=percents]").Change(Percents.ToString());
            cut.Find("form").Submit();

            cut.FindComponent<CircleLoader>();
        }

        [Fact]
        public async Task MainPage_UserClicksBackButton_DepositeFormReplacesResultList()
        {
            var cut = RenderComponent<MainPage>();

            cut.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            cut.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            cut.Find("input[id=percents]").Change(Percents.ToString());
            cut.Find("form").Submit();

            await Task.Delay(200);
            cut.Find("button").Click();

            cut.FindComponent<DepositeForm>(); // тестируем инпуты
        }
    }
}
