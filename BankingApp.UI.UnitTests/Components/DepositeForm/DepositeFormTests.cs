using Bunit;
using Xunit;

namespace BankingApp.UI.UnitTests.Components.DepositeForm
{
    public class DepositeFormTests : TestContext
    {
        [Fact]
        public void DepositeForm_UserSubmitsValidData_CallbackTriggersAndSetsValidDataToStateVariable()
        {
            var cut = RenderComponent<BankingApp.UI.Components.DepositeForm.DepositeForm>();

            const int DepositeSum = 100;
            const int MonthsCount = 12;
            const int Percents = 10;

            cut.Find("input[id=depositeSum]").Change(DepositeSum.ToString());
            cut.Find("input[id=monthCount]").Change(MonthsCount.ToString());
            cut.Find("input[id=percents]").Change(Percents.ToString());
            cut.Find("form").Submit();

            var formModel = cut.Instance._requestModel;

            Assert.Equal(formModel.DepositeSum, DepositeSum);
            Assert.Equal(formModel.MonthsCount, MonthsCount);
            Assert.Equal(formModel.Percents, Percents);
        }
    }
}
