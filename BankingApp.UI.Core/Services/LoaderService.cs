using BankingApp.UI.Core.Interfaces;
using System;

namespace BankingApp.UI.Core.Services
{
    /// <summary>
    /// Allows to show / hide fullscreen loader animation. Used while program is busy.
    /// </summary>
    public class LoaderService : ILoaderService
    {
        /// <summary>
        /// Triggers when switcher state was changed.
        /// </summary>
        public event Action<bool> OnLoaderSwitch;

        /// <summary>
        /// Hides fullscreen loader animation.
        /// </summary>
        public void SwitchOff()
        {
            OnLoaderSwitch?.Invoke(false);
        }

        /// <summary>
        /// Shows fullscreen loader animation.
        /// </summary>
        public void SwitchOn()
        {
            OnLoaderSwitch?.Invoke(true);
        }
    }
}
