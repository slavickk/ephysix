using ParserLibrary;
using System;

namespace WebApplicationConfigUI1.Client
{
    public class StateContainer
    {
        private Pipeline savedString;

        public Pipeline Property
        {
            get => savedString;
            set
            {
                savedString = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
