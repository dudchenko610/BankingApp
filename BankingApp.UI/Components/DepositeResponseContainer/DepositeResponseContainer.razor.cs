using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositeResponseContainer
{
    public partial class DepositeResponseContainer
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public EventCallback OnBackToForm { get; set; }

        private void BackToForm()
        {
            OnBackToForm.InvokeAsync(null);
        }
    }
}
