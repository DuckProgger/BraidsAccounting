﻿using BraidsAccounting.Services.Interfaces;
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

        public void ActivateWindowWithClosing<TOpenedWindow, TClosedWindow>(Action? action = null)
            where TOpenedWindow : Window, new()
            where TClosedWindow : Window, new()
        {
            TOpenedWindow openedWindow = new();
            var closedWindow = GetWindow<TClosedWindow>();
            // Открыть окно, которое было закрыто, при закрытии окна, которое было открыто
            openedWindow.Closed += (s, e) => closedWindow.Show();
            openedWindow.Closed += (s, e) => action?.Invoke();
            closedWindow.Hide();
            openedWindow.Show();
        }

        public T GetWindow<T>() => Application.Current.Windows.OfType<T>().First();
    }
}