using BankingApp.UI.Components.InputSelectEnum;
using BankingApp.ViewModels.Enums;
using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

using BankingApp.Shared.Extensions;
using BankingApp.Shared.Helpers;
using FluentAssertions;

namespace BankingApp.UI.UnitTests.Components
{
    public class InputSelectEnumTests : TestContext
    {
        [Fact]
        public void InputSelectEnum_PassEnumWithAttributeNames_OptionsContainsAttributeNames()
        {
            var editContext = new EditContext(new { });
            var myEnum = DepositeCalculationFormulaEnumView.CompoundInterest;

            var inputSelectEnum = RenderComponent<InputSelectEnum<DepositeCalculationFormulaEnumView>>(parameters => parameters
                .AddCascadingValue(editContext)
                .Add(p => p.ValueExpression, () => myEnum)
            );

            var optionsTextList = inputSelectEnum.FindAll("option").Select(x => x.TextContent).ToList();
            var enumDisplayNames = EnumHelper.GetDisplayValues<DepositeCalculationFormulaEnumView>();

            optionsTextList.Should().BeEquivalentTo(enumDisplayNames);
        }
    }
}
