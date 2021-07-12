using BankingApp.UI.Core.Interfaces;
using System;

namespace BankingApp.UI.Core.Services
{
    public class LoaderService : ILoaderService
    {
        public event Action<bool> OnLoaderSwitch;

        public void SwitchOff()
        {
            OnLoaderSwitch?.Invoke(false);
        }

        public void SwitchOn()
        {
            OnLoaderSwitch?.Invoke(true);
        }
    }
}
