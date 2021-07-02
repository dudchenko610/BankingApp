﻿using BankingApp.UI.Components.CircleLoader;
using BankingApp.UI.Components.DepositeForm;
using BankingApp.UI.Core.Enums;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.DepositePage;
using BankingApp.ViewModels.Banking;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BankingApp.UI.UnitTests.Pages
{
    public class DepositePageTests : TestContext
    {
        private class MockDepositeService : IDepositeService
        {
            public async Task<ResponseCalculateDepositeBankingView> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDeposite)
            {
                await Task.Delay(100); // simulating api call
                return new ResponseCalculateDepositeBankingView();
            }
        }

        public DepositePageTests()
        {
            Services.AddSingleton<IDepositeService>(new MockDepositeService());
        }

        [Fact]
        public void DepositePage_UserSubmitsValidData_PageContentReplacesWithLoader()
        {
            var cut = RenderComponent<DepositePage>();

            const int DepositeSum = 100;
            const int MonthsCount = 12;
            const int Percents = 10;

            cut.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            cut.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            cut.Find("input[id=percents]").Change(Percents.ToString());
            cut.Find("form").Submit();

            cut.FindComponent<CircleLoader>();
        }

        [Fact]
        public async Task DepositePage_UserClickBackButton_DepositeFormReplacesResultList()
        {
            var cut = RenderComponent<DepositePage>();

            const int DepositeSum = 100;
            const int MonthsCount = 12;
            const int Percents = 10;

            cut.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            cut.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            cut.Find("input[id=percents]").Change(Percents.ToString());
            cut.Find("form").Submit();

            await Task.Delay(200);
            cut.Find("button").Click();

            cut.FindComponent<DepositeForm>();
        }
    }
}