using BankingApp.ViewModels.Banking;
using Bunit;
using Xunit;

namespace BankingApp.UI.UnitTests.Components.DepositeForm
{
    public class DepositeFormTests : TestContext
    {

        // for routing use mock of NavManager and Verify method
        [Fact]
        public void DepositeForm_UserSubmitsValidData_CallbackTriggersAndSetsValidDataToStateVariable()
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

            Assert.NotNull(model);
            Assert.Equal(model.DepositeSum, DepositeSum);
            Assert.Equal(model.MonthsCount, MonthsCount);
            Assert.Equal(model.Percents, Percents);
        }
    }
}
