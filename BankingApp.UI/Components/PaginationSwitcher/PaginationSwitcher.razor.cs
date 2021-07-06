using Microsoft.AspNetCore.Components;
using System;

namespace BankingApp.UI.Components.PaginationSwitcher
{
    public partial class PaginationSwitcher
    {
        [Parameter]
        public int PageCount { get; set; }
        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public EventCallback<int> OnPageClick { get; set; }

        private int _pageOffset;
        private int _selectedPage;

        private string _prevDisabledClass;
        private string _nextDisabledClass;

        private string _1DisabledClass;
        private string _2DisabledClass;
        private string _3DisabledClass;

        public PaginationSwitcher()
        {
            _pageOffset = 0;
            _prevDisabledClass = "";
            _nextDisabledClass = "";

            _1DisabledClass = "";
            _2DisabledClass = "";
            _3DisabledClass = "";
        }

        protected override void OnInitialized()
        {
            _pageOffset = CurrentPage - 2;
            _selectedPage = CurrentPage < 1 || CurrentPage > PageCount ? 1 : CurrentPage;
            
            RecalculateConstraints();
            StateHasChanged();
        }


        private void RecalculateConstraints()
        {
            _prevDisabledClass = "";
            _nextDisabledClass = "";
            _1DisabledClass = "";
            _2DisabledClass = "";
            _3DisabledClass = "";

            switch (PageCount)
            {
                case 1:
                    _2DisabledClass = "disabled";
                    _3DisabledClass = "disabled";
                    _prevDisabledClass = "disabled";
                    _nextDisabledClass = "disabled";
                    _pageOffset = 0;
                    return;
                case 2:
                    _3DisabledClass = "disabled";
                    _prevDisabledClass = "disabled";
                    _nextDisabledClass = "disabled";
                    _pageOffset = 0;
                    return;
                case 3:
                    _prevDisabledClass = "disabled";
                    _nextDisabledClass = "disabled";
                    _pageOffset = 0;
                    return;
            }

            if (_pageOffset <= 0)
            {
                _prevDisabledClass = "disabled";
                _pageOffset = 0;
            }

            if ((_pageOffset + 3) >= PageCount) 
            {
                _nextDisabledClass = "disabled";
                _pageOffset = PageCount - 3;
            }
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

        private string IsButtonActive(int i)
        {
            int checking = _pageOffset + i + 1;
            return checking == _selectedPage ? "active" : "";
        }

        private void OnPageSelected(int i)
        {
            _selectedPage = _pageOffset + i + 1;
            OnPageClick.InvokeAsync(_selectedPage);
        }
    }
}
