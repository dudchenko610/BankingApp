using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.NotificationPage
{
    public partial class NotificationPage
    {
        [Parameter]
        public string Message { get; set; }
    }
}
