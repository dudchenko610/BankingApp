using BankingApp.ViewModels.Banking;
using Bunit;
using Xunit;
using FluentAssertions;
using BankingApp.UI.Components.DepositeForm;

namespace BankingApp.UI.UnitTests.Components
{
    public class DepositeFormTests : TestContext
    {
        [Fact]
        public void DepositeForm_UserSubmitsValidData_CallbackTriggersAndReturnsValidData()
        {
            const int DepositeSum = 100;
            const int MonthsCount = 12;
            const int Percents = 10;

            RequestCalculateDepositeBankingView model = null;

            var depositeForm = RenderComponent<DepositeForm>(
                parameters => parameters.Add(component => component.OnFormSubmit,
                    (formModel) => { model = formModel; }
                )
            );

            depositeForm.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            depositeForm.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            depositeForm.Find("input[id=percents]").Change(Percents.ToString());
            depositeForm.Find("form").Submit();

            model.Should().NotBeNull();
            model.DepositeSum.Should().Be(DepositeSum);
            model.MonthsCount.Should().Be(MonthsCount);
            model.Percents.Should().Be(Percents);
        }
    }
}
