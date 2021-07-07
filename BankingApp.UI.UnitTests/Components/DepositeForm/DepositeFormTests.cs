using BankingApp.ViewModels.Banking;
using Bunit;
using Xunit;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Components.DepositeForm
{
    public class DepositeFormTests : TestContext
    {

        // for routing use mock of NavManager and Verify method
        [Fact]
        public void DepositeForm_UserSubmitsValidData_CallbackTriggersAndReturnsValidData()
        {
            const int DepositeSum = 100;
            const int MonthsCount = 12;
            const int Percents = 10;

            RequestCalculateDepositeBankingView model = null;

            var depositeForm = RenderComponent<BankingApp.UI.Components.DepositeForm.DepositeForm>(
                parameters => parameters.Add(component => component.OnFormSubmit, 
                    (formModel) => { model = formModel; }
                )
            );

            depositeForm.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            depositeForm.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            depositeForm.Find("input[id=percents]").Change(Percents.ToString());
            depositeForm.Find("form").Submit();

            model.Should().NotBeNull();
            model.DepositeSum.Should().Equals(DepositeSum);
            model.MonthsCount.Should().Equals(MonthsCount);
            model.Percents.Should().Equals(Percents);
        }
    }
}
