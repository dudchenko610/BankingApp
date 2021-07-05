using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Components.PaginationSwitcher
{
    public partial class PaginationSwitcher
    {

        [Parameter]
        public int PageCount { get; set; }
        private int _pageOffset;

        private string _prevDisabledClass;
        private string _nextDisabledClass;

        public PaginationSwitcher()
        {
            _pageOffset = 0;
            _prevDisabledClass = "";
            _nextDisabledClass = "";
        }

        protected override void OnInitialized()
        {
            RecalculateConstraints();
        }

        private void RecalculateConstraints()
        {
            _prevDisabledClass = _pageOffset == 0 ? "disabled" : "";
            _nextDisabledClass = (_pageOffset + 3) == PageCount ? "disabled" : "";
        }

        private void OnNextClick()
        {
            _pageOffset += 1;
            RecalculateConstraints();
        }
        private void OnPrevClick()
        {
            _pageOffset -= 1;
            RecalculateConstraints();
        }
    }
}
