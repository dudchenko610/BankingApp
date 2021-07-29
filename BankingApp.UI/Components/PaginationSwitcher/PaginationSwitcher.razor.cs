using Microsoft.AspNetCore.Components;
using System;

namespace BankingApp.UI.Components.PaginationSwitcher
{
    public partial class PaginationSwitcher
    {
        private const string DisabledClass = "disabled";
        private const string ActiveClass = "active";

        private int _pageOffset;

        private string _prevDisabledClass;
        private string _nextDisabledClass;

        private string _1DisabledClass;
        private string _2DisabledClass;
        private string _3DisabledClass;

        [Parameter]
        public int PageCount { get; set; }
        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public EventCallback<int> OnPageClick { get; set; }

        public PaginationSwitcher()
        {
            _pageOffset = 0;
            _prevDisabledClass = string.Empty;
            _nextDisabledClass = string.Empty;

            _1DisabledClass = string.Empty;
            _2DisabledClass = string.Empty;
            _3DisabledClass = string.Empty;
        }

        protected override void OnInitialized()
        {
            if (PageCount <= 1)
            {
                PageCount = 1;
            }

            CurrentPage = CurrentPage < 1 || CurrentPage > PageCount ? 1 : CurrentPage;
            _pageOffset = CurrentPage - 2;
            
            RecalculateConstraints();
            StateHasChanged();
        }


        private void RecalculateConstraints()
        {
            _prevDisabledClass = string.Empty;
            _nextDisabledClass = string.Empty;
            _1DisabledClass = string.Empty;
            _2DisabledClass = string.Empty;
            _3DisabledClass = string.Empty;

            switch (PageCount)
            {
                case 1:
                    _2DisabledClass = DisabledClass;
                    _3DisabledClass = DisabledClass;
                    _prevDisabledClass = DisabledClass;
                    _nextDisabledClass = DisabledClass;
                    _pageOffset = 0;
                    return;
                case 2:
                    _3DisabledClass = DisabledClass;
                    _prevDisabledClass = DisabledClass;
                    _nextDisabledClass = DisabledClass;
                    _pageOffset = 0;
                    return;
                case 3:
                    _prevDisabledClass = DisabledClass;
                    _nextDisabledClass = DisabledClass;
                    _pageOffset = 0;
                    return;
            }

            if (_pageOffset <= 0)
            {
                _prevDisabledClass = DisabledClass;
                _pageOffset = 0;
            }

            if ((_pageOffset + 3) >= PageCount) 
            {
                _nextDisabledClass = DisabledClass;
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

            return checking == CurrentPage ? ActiveClass : string.Empty;
        }

        private void OnPageSelected(int i)
        {
            CurrentPage = _pageOffset + i + 1;
            OnPageClick.InvokeAsync(CurrentPage);
        }
    }
}
