using System.Windows;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IViewService
    {
        void ActivateWindow<TWindowType>() where TWindowType : Window, new();
        void ActivateWindowWithClosing<TOpenedWindow, TClosedWindow>()
              where TOpenedWindow : Window, new()
              where TClosedWindow : Window, new();
    }
}