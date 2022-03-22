using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BraidsAccounting.Services
{
    internal class ViewService : IViewService
    {
        public void ActivateWindow<TWindowType>() where TWindowType : Window, new()
        {
            new TWindowType().Show();
        }

        public void ActivateWindowWithClosing<TOpenedWindow, TClosedWindow>()
            where TOpenedWindow : Window, new()
            where TClosedWindow : Window, new()
        {
            TOpenedWindow openedWindow = new();
            var closedWindow = Application.Current.Windows.OfType<TClosedWindow>().First();
            // Открыть окно, которое было закрыто, при закрытии окна, которое было открыто
            openedWindow.Closed += (s, e) => closedWindow.Show();
            closedWindow.Hide();
            openedWindow.Show();
        }
    }
}
