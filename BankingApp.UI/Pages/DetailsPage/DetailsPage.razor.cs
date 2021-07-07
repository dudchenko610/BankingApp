using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Pages.DetailsPage
{
    public partial class DetailsPage
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }
        [Parameter]
        public int DepositeHistory { get; set; }


    }
}
