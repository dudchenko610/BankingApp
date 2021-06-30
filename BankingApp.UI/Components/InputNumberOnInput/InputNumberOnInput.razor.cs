using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace BankingApp.UI.Components.InputNumberOnInput
{
    public partial class InputNumberOnInput<T> : InputNumber<T>
    {
        private string stringValue;
        private T lastParsedValue;

        protected override void OnParametersSet()
        {
            if (!Equals(CurrentValue, lastParsedValue))
            {
                lastParsedValue = CurrentValue;
                stringValue = CurrentValueAsString;
            }
        }

        private void OnInput(ChangeEventArgs e)
        {
            CurrentValueAsString = stringValue = (string)e.Value;
            lastParsedValue = CurrentValue;
        }

        private void OnBlur(FocusEventArgs e)
        {
            if (!EditContext.GetValidationMessages(FieldIdentifier).Any())
            {
                stringValue = CurrentValueAsString;
            }
        }
    }
}
