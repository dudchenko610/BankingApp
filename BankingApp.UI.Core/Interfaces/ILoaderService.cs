
using System;

namespace BankingApp.UI.Core.Interfaces
{
    public interface ILoaderService
    {
        event Action<bool> OnLoaderSwitch;
        void SwitchOn();
        void SwitchOff();
    }
}
