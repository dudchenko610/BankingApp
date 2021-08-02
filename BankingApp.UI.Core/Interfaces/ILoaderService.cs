using System;

namespace BankingApp.UI.Core.Interfaces
{
    /// <summary>
    /// Allows to show / hide fullscreen loader animation. Used while program is busy.
    /// </summary>
    public interface ILoaderService
    {
        /// <summary>
        /// Triggers when switcher state was changed.
        /// </summary>
        event Action<bool> OnLoaderSwitch;

        /// <summary>
        /// Shows fullscreen loader animation.
        /// </summary>
        void SwitchOn();

        /// <summary>
        /// Hides fullscreen loader animation.
        /// </summary>
        void SwitchOff();
    }
}
