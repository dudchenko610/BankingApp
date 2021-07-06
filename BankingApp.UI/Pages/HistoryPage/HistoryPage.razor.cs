using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.HistoryPage
{
    public partial class HistoryPage
    {
        [Inject]
        private IDepositeService _depositeService { get; set; }

        protected void OnPageClicked(int page)
        { 
        
        }
    }
}
