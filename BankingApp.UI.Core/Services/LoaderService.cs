using BankingApp.UI.Core.Interfaces;
using System;

namespace BankingApp.UI.Core.Services
{
    public class LoaderService : ILoaderService
    {
        private int _switchCounter;
        public event Action<bool> OnLoaderSwitch;

        public LoaderService()
        {
            _switchCounter = 0;
        }

        public void SwitchOff()
        {
            _switchCounter--;
            _switchCounter = _switchCounter < 0 ? 0 : _switchCounter;

            if (_switchCounter == 0)
                OnLoaderSwitch?.Invoke(false);
        }

        public void SwitchOn()
        {
            int prevSwitch = _switchCounter;
            _switchCounter++;
            if (prevSwitch == 0)
                OnLoaderSwitch?.Invoke(true);
        }
    }
}
