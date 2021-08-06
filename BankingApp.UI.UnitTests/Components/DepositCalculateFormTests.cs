using Bunit;
using Xunit;
using FluentAssertions;
using BankingApp.UI.Components.DepositCalculateForm;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Deposit;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositCalculateFormTests : TestContext
    {
        private const decimal ValidDepositSum = 100;
        private const int ValidMonthsCount = 12;
        private const float ValidPercents = 10; 

        [Fact]
        public void WhenTheFormIsSubmited_UserSubmitsValidData_OnFormSubmitInvoked()
        {
            CalculateDepositView depositViewModel = null;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>(
                parameters => parameters.Add(component => component.OnFormSubmit,
                    (formModel) => { depositViewModel = formModel; }
                )
            );

            depositCalculateForm.Find("input[id=depositSum]").Change(ValidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositCalculateForm.Find("form").Submit();

            depositViewModel.Should().NotBeNull();
            depositViewModel.DepositSum.Should().Be(ValidDepositSum);
            depositViewModel.MonthsCount.Should().Be(ValidMonthsCount);
            depositViewModel.Percents.Should().Be((decimal)ValidPercents);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserTypesNegativeDepositSum_ExpectedMarkupRendered()
        {
            const decimal InvalidDepositSum = -55.55m;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>();

            depositCalculateForm.Find("input[id=depositSum]").Change(InvalidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositCalculateForm.Render();

            var validationErrorMessage = depositCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPriceFormat);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserTypesDepositSumWithMoreThanTwoDecimalPlaces_ExpectedMarkupRendered()
        {
            const decimal InvalidDepositSum = 95.589m;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>();

            depositCalculateForm.Find("input[id=depositSum]").Change(InvalidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositCalculateForm.Render();

            var validationErrorMessage = depositCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPriceFormat);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserTypesPercentLessThanOne_ExpectedMarkupRendered()
        {
            const float InvalidPercents = 0;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>();

            depositCalculateForm.Find("input[id=depositSum]").Change(ValidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(InvalidPercents.ToString());
            depositCalculateForm.Render();

            var validationErrorMessage = depositCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPercentNumber);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserTypesPercentBiggerThan100_ExpectedMarkupRendered()
        {
            const float InvalidPercents = 101;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>();

            depositCalculateForm.Find("input[id=depositSum]").Change(ValidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(InvalidPercents.ToString());
            depositCalculateForm.Render();

            var validationErrorMessage = depositCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPercentNumber);
        }
        
        [Fact]
        public void WhenTheComponentIsRendered_UserTypesPercentsWithMoreThanTwoDecimalPlaces_ExpectedMarkupRendered()
        {
            const decimal InvalidPercents = 95.589m;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>();

            depositCalculateForm.Find("input[id=depositSum]").Change(ValidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(InvalidPercents.ToString());
            depositCalculateForm.Render();

            var validationErrorMessage = depositCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPercentNumber);
        }

        [Fact]
        public void WhenTheComponentIsRendered_UserTypesMonthCountLessThan1_ExpectedMarkupRendered()
        {
            const int InvalidMonthCount = 0;

            var depositCalculateForm = RenderComponent<DepositCalculateForm>();

            depositCalculateForm.Find("input[id=depositSum]").Change(ValidDepositSum.ToString());
            depositCalculateForm.Find("input[id=monthCount]").Change(InvalidMonthCount.ToString());
            depositCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositCalculateForm.Render();

            var validationErrorMessage = depositCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectMonthFormat);
        }
    }
}
