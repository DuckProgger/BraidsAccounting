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
        //public void ShowWindow<TWindowType>() where TWindowType : Window, new() => new TWindowType().Show();

        //public void ShowWindowWithClosing<TOpenedWindow, TClosedWindow>(Action? action = null)
        //    where TOpenedWindow : Window, new()
        //    where TClosedWindow : Window, new()
        //{
        //    TOpenedWindow openedWindow = new();
        //    TClosedWindow? closedWindow = GetWindow<TClosedWindow>();
        //    // Открыть окно, которое было закрыто, при закрытии окна, которое было открыто
        //    openedWindow.Closed += (s, e) => closedWindow.Show();
        //    openedWindow.Closed += (s, e) => action?.Invoke();
        //    closedWindow.Hide();
        //    openedWindow.Show();
        //}

        private static T GetWindow<T>() where T : Window =>
            Application.Current.Windows.OfType<T>().First();

        private Stack<WindowInfo> windowsStack = new();
        private IRegionNavigationJournal journal = null!;

        //public ViewService()
        //{
        //    //windowsStack = new();
        //    //var windowInfo = new WindowInfo()
        //    //{
        //    //    Window = GetWindow<MainWindow>(),
        //    //    RegionManager = ServiceLocator.GetService<IRegionManager>(),
        //    //    ViewName = nameof(MainWindow)
        //    //};
        //    //windowsStack.Push(windowInfo);
        //}

        public void ShowPopupWindow(string viewName, NavigationParameters? parameters = null, Action? callback = null)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName), "Название переключаемого представления не может быть пустым.");
            //var currentWindowInfo = windowsStack.Peek();
            if (windowsStack.Count == 0)
            {
                CreatePopupWindow(viewName, parameters, callback);
                return;
            }
            var currentWindowInfo = windowsStack.Peek();
            WindowInfo newWindowInfo = new()
            {
                RegionManager = currentWindowInfo.RegionManager,
                ViewName = viewName,
            };
            windowsStack.Push(newWindowInfo);
            newWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
            journal = newWindowInfo.RegionManager.Regions[RegionNames.Popup].NavigationService.Journal;
        }

        private void CreatePopupWindow(string viewName, NavigationParameters? parameters = null, Action? callback = null)
        {
            var mainWindow = GetWindow<MainWindow>();
            PopupWindow popupWindow = new();
            popupWindow.Closed += (s, e) => mainWindow.Show();
            popupWindow.Closed += (s, e) => ClearStackAndInvokeFirstCallback();
            mainWindow.Hide();
            popupWindow.Show();
            var regionManager = ServiceLocator.GetService<IRegionManager>();
            var scopedRegionManager = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(popupWindow, scopedRegionManager);
            var popupWindowInfo = new WindowInfo()
            {
                //Window = popupWindow,
                RegionManager = scopedRegionManager,
                ViewName = viewName,
                Callback = callback
            };
            windowsStack.Push(popupWindowInfo);
            popupWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
            journal = popupWindowInfo.RegionManager.Regions[RegionNames.Popup].NavigationService.Journal;
        }

        public void GoBack()
        {
            windowsStack.Pop();
            journal.GoBack();
        }

        public void GoBack(NavigationParameters parameters)
        {
            var currentWindowInfo = windowsStack.Pop();
            var prevViewName = windowsStack.Peek().ViewName;
            currentWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, prevViewName, parameters);
            currentWindowInfo.Callback?.Invoke();
            //journal.GoBack();
        }

        private void ClearStackAndInvokeFirstCallback()
        {
            while (windowsStack.Count > 1)
                windowsStack.Pop();
            windowsStack.Pop().Callback?.Invoke();
        }

        //public string GetUri()
        //{
        //    string uri = string.Empty;
        //    foreach (var windowInfo in windowsStack)
        //        uri += windowInfo.Window.Title;
        //    return uri;
        //}

        public void ClosePopupWindow()
        {
            var popupWindow = GetWindow<PopupWindow>();
            popupWindow.Close();
            //windowsStack.Peek().Window.Close();
            //windowsStack.Clear();
        }

        private class WindowInfo
        {
            //public Window Window { get; set; } = null!;
            public IRegionManager RegionManager { get; set; } = null!;
            public string ViewName { get; set; } = null!;
            public Action? Callback { get; set; }
        }
    }
}
