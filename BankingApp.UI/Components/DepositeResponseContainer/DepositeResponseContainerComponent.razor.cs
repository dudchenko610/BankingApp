using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Components.DepositeResponseContainer
{
    public partial class DepositeResponseContainerComponent
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
