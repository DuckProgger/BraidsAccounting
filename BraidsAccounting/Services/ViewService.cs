using BraidsAccounting.Modules;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Реализация сервиса <see cref = "IViewService" />.
    /// </summary>
    internal class ViewService : IViewService
    {
        public void ShowWindow<TWindowType>() where TWindowType : Window, new() => new TWindowType().Show();

        public void ShowWindowWithClosing<TOpenedWindow, TClosedWindow>(Action? action = null)
            where TOpenedWindow : Window, new()
            where TClosedWindow : Window, new()
        {
            TOpenedWindow openedWindow = new();
            TClosedWindow? closedWindow = GetWindow<TClosedWindow>();
            // Открыть окно, которое было закрыто, при закрытии окна, которое было открыто
            openedWindow.Closed += (s, e) => closedWindow.Show();
            openedWindow.Closed += (s, e) => action?.Invoke();
            closedWindow.Hide();
            openedWindow.Show();
        }

        public T GetWindow<T>() where T : Window =>
            Application.Current.Windows.OfType<T>().First();

        //public void ShowPopupWindow<TCurrentWindow>(string viewName, Action? action = null, NavigationParameters? parameters = null)
        //    where TCurrentWindow : Window, new()
        //{
        //    if (string.IsNullOrEmpty(viewName))
        //        throw new ArgumentNullException(nameof(viewName), "Название переключаемого представления не может быть пустым.");
        //    ShowWindowWithClosing<PopupWindow, TCurrentWindow>(action);
        //    var regionManager = ServiceLocator.GetService<IRegionManager>();
        //    var scopedRegionManager = regionManager.CreateRegionManager();
        //    RegionManager.SetRegionManager(GetWindow<PopupWindow>(), scopedRegionManager);
        //    scopedRegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
        //}

        public void NavigateToPreviousWindow<TCurrentWindow, TPrevWindow>(NavigationParameters? parameters = null)
              where TCurrentWindow : Window, new()
        {
            GetWindow<TCurrentWindow>().Close();
            var regionManager = ServiceLocator.GetService<IRegionManager>();

        }

        //private static IEnumerable<T> GetWindows<T>() where T : Window =>
        //    Application.Current.Windows.OfType<T>();

        //private void ShowNestedPopupWindow<TClosedView>(Action? action = null)
        //{
        //    PopupWindow openedWindow = new();
        //    var popupWindows = GetWindows<PopupWindow>();
        //    var closedWindows = popupWindows
        //        .Where(w => ((w.Content as Grid).Children[0] as ContentControl).Content.GetType() == typeof(TClosedView));
        //    if (closedWindows.Count() > 1) throw new Exception("Открываемый тип представления уже используется.");
        //    var currentWindow = closedWindows.First();
        //    openedWindow.Closed += (s, e) => currentWindow.Show();
        //    openedWindow.Closed += (s, e) => action?.Invoke();
        //    currentWindow.Hide();
        //    openedWindow.Show();
        //    //DialogService
        //}

        private Stack<Window> windowsStack;
        //private LinkedList<Window> windowsLl;


        public ViewService()
        {
            windowsStack = new();
            windowsStack.Push(GetWindow<MainWindow>());
        }

        public void ShowPopupWindow(string viewName, Action<object>? callback = null, NavigationParameters? parameters = null)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName), "Название переключаемого представления не может быть пустым.");
            Window? closedWindow = windowsStack.Peek();
            PopupWindow openedWindow = new();
            windowsStack.Push(openedWindow);
            openedWindow.Closed += (s, e) => closedWindow.Show();
            openedWindow.Closed += (s, e) => callback?.Invoke(openedWindow.Result);
            openedWindow.Closed += (s, e) => windowsStack.Pop();
            closedWindow.Hide();
            openedWindow.Show();
            var regionManager = ServiceLocator.GetService<IRegionManager>();
            var scopedRegionManager = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(openedWindow, scopedRegionManager);
            scopedRegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
        }

        public string GetUri()
        {
            string uri = string.Empty;
            foreach (var window in windowsStack)
                uri += window.Title;
            return uri;
        }

    }
}
