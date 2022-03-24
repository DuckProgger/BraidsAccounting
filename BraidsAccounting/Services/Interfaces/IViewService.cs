using System;
using System.Windows;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IViewService
    {
        void ActivateWindow<TWindowType>() where TWindowType : Window, new();
        public void ActivateWindowWithClosing<TOpenedWindow, TClosedWindow>(Action? action = null)
              where TOpenedWindow : Window, new()
              where TClosedWindow : Window, new();
        T GetWindow<T>();
    }
}