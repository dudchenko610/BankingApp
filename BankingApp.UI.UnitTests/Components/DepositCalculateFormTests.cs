using Bunit;
using Xunit;
using FluentAssertions;
using BankingApp.UI.Components.DepositCalculateForm;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Deposit;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositCalculateFormTests : TestContext
    {
        private const decimal ValidDepositeSum = 100;
        private const int ValidMonthsCount = 12;
        private const float ValidPercents = 10; 

        [Fact]
        public void DepositCalculateForm_UserSubmitsValidData_CallbackTriggersAndReturnsValidData()
        {
            CalculateDepositView depositeViewModel = null;

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>(
                parameters => parameters.Add(component => component.OnFormSubmit,
                    (formModel) => { depositeViewModel = formModel; }
                )
            );

            depositeCalculateForm.Find("input[id=depositeSum]").Change(ValidDepositeSum.ToString());
            depositeCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositeCalculateForm.Find("form").Submit();

            depositeViewModel.Should().NotBeNull();
            depositeViewModel.DepositSum.Should().Be(ValidDepositeSum);
            depositeViewModel.MonthsCount.Should().Be(ValidMonthsCount);
            depositeViewModel.Percents.Should().Be((decimal)ValidPercents);
        }

        [Fact]
        public void DepositCalculateForm_UserTypesNegativeDepositeSum_RenderTreeContainsIncorrectPriceFormatMessage()
        {
            const decimal InvalidDepositeSum = -55.55m;

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>();

            depositeCalculateForm.Find("input[id=depositeSum]").Change(InvalidDepositeSum.ToString());
            depositeCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositeCalculateForm.Render();

            var validationErrorMessage = depositeCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPriceFormat);
        }

        [Fact]
        public void DepositCalculateForm_UserTypesDepositeSumWithMoreThanTwoDecimalPlaces_RenderTreeContainsIncorrectPriceFormatMessage()
        {
            const string InvalidDepositeSum = "33,454";

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>();

            depositeCalculateForm.Find("input[id=depositeSum]").Change(InvalidDepositeSum);
            depositeCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositeCalculateForm.Render();

            var validationErrorMessage = depositeCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPriceFormat);
        }

        [Fact]
        public void DepositCalculateForm_UserTypesPercentLessThanOne_RenderTreeContainsIncorrectPercentNumberMessage()
        {
            const float InvalidPercents = 0;

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>();

            depositeCalculateForm.Find("input[id=depositeSum]").Change(ValidDepositeSum.ToString());
            depositeCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(InvalidPercents.ToString());
            depositeCalculateForm.Render();

            var validationErrorMessage = depositeCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPercentNumber);
        }

        [Fact]
        public void DepositCalculateForm_UserTypesPercentBiggerThan100_RenderTreeContainsIncorrectPercentNumberMessage()
        {
            const float InvalidPercents = 101;

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>();

            depositeCalculateForm.Find("input[id=depositeSum]").Change(ValidDepositeSum.ToString());
            depositeCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(InvalidPercents.ToString());
            depositeCalculateForm.Render();

            var validationErrorMessage = depositeCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPercentNumber);
        }
        
        [Fact]
        public void DepositCalculateForm_UserTypesPercentsWithMoreThanTwoDecimalPlaces_RenderTreeContainsIncorrectPercentNumberMessage()
        {
            const decimal InvalidPercents = 95.589m;

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>();

            depositeCalculateForm.Find("input[id=depositeSum]").Change(ValidDepositeSum.ToString());
            depositeCalculateForm.Find("input[id=monthCount]").Change(ValidMonthsCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(InvalidPercents.ToString());
            depositeCalculateForm.Render();

            var validationErrorMessage = depositeCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectPercentNumber);
        }

        [Fact]
        public void DepositCalculateForm_UserTypesMonthCountLessThan1_RenderTreeContainsIncorrectMonthFormatMessage()
        {
            const int InvalidMonthCount = 0;

            var depositeCalculateForm = RenderComponent<DepositCalculateForm>();

            depositeCalculateForm.Find("input[id=depositeSum]").Change(ValidDepositeSum.ToString());
            depositeCalculateForm.Find("input[id=monthCount]").Change(InvalidMonthCount.ToString());
            depositeCalculateForm.Find("input[id=percents]").Change(ValidPercents.ToString());
            depositeCalculateForm.Render();

            var validationErrorMessage = depositeCalculateForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Deposit.IncorrectMonthFormat);
        }
    }
}
