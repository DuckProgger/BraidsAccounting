using BraidsAccounting.Modules;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Реализация сервиса <see cref = "IViewService" />.
    /// </summary>
    internal class ViewService : IViewService
    {
        private readonly LinkedList<WindowInfo> windowsStack = new();
        private IRegionNavigationJournal journal = null!;

        public void ShowPopupWindow(string viewName, NavigationParameters? parameters = null, Action<NavigationParameters?>? callback = null)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName), "Название переключаемого представления не может быть пустым.");
            if (windowsStack.Count == 0)
            {
                CreatePopupWindow(viewName, parameters, callback);
                return;
            }
            WindowInfo? currentWindowInfo = windowsStack.Last();
            WindowInfo newWindowInfo = new()
            {
                RegionManager = currentWindowInfo.RegionManager,
                ViewName = viewName,
            };
            windowsStack.AddLast(newWindowInfo);
            newWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
            journal = newWindowInfo.RegionManager.Regions[RegionNames.Popup].NavigationService.Journal;
        }

        public void GoBack()
        {
            //windowsStack.Pop();
            //journal.GoBack();

            WindowInfo? currentLayer = windowsStack.Last();
            windowsStack.RemoveLast();
            currentLayer.Callback?.Invoke(currentLayer.Parameters);
            if (windowsStack.Count == 0)
            {
                ClosePopupWindow();
                return;
            }
            string? prevViewName = windowsStack.Last().ViewName;
            currentLayer.RegionManager.RequestNavigate(RegionNames.Popup, prevViewName, currentLayer.Parameters);
        }

        //public void GoBack(NavigationParameters parameters)
        //{
        //    WindowInfo? currentWindowInfo = windowsStack.Last();
        //    windowsStack.RemoveLast();
        //    string? prevViewName = windowsStack.Last().ViewName;
        //    currentWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, prevViewName, parameters);
        //    currentWindowInfo.Callback?.Invoke();
        //}
        public void AddParameter(string name, object value)
        {
            var lastLayer = windowsStack.Last();
            if (lastLayer.Parameters is null)
                lastLayer.Parameters = new();
            lastLayer.Parameters.Add(name, value);
        }

        private static void ClosePopupWindow()
        {
            PopupWindow? popupWindow = GetWindow<PopupWindow>();
            popupWindow.Close();
        }        

        private void CreatePopupWindow(
            string viewName, NavigationParameters? parameters = null, Action<NavigationParameters?>? callback = null)
        {
            MainWindow? mainWindow = GetWindow<MainWindow>();
            PopupWindow popupWindow = new();
            popupWindow.Closed += (s, e) => mainWindow.Show();
            //popupWindow.Closed += (s, e) => ClearStackAndInvokeFirstCallback();   
            popupWindow.Closed += (s, e) => windowsStack.Clear();
            mainWindow.Hide();
            popupWindow.Show();
            IRegionManager? regionManager = ServiceLocator.GetService<IRegionManager>();
            IRegionManager? scopedRegionManager = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(popupWindow, scopedRegionManager);
            WindowInfo? popupWindowInfo = new WindowInfo()
            {
                RegionManager = scopedRegionManager,
                ViewName = viewName,
                Callback = callback
            };
            windowsStack.AddLast(popupWindowInfo);
            popupWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
            journal = popupWindowInfo.RegionManager.Regions[RegionNames.Popup].NavigationService.Journal;
        }

        //private void ClearStackAndInvokeFirstCallback()
        //{
        //    //while (windowsStack.Count > 1)
        //    //    windowsStack.Pop();
        //    var firstWindowInfo = windowsStack.First();
        //    firstWindowInfo.Callback?.Invoke(firstWindowInfo.Parameters);
        //    windowsStack.Clear();
        //    //// Для первого слоя произвести обраный вызов метода и подставить значения из слоя выше
        //    //firstWindowInfo.Callback?.Invoke(windowsStack.First?.Next?.Value.Parameters);
        //    //return firstWindowInfo.Parameters;
        //}


        private static T GetWindow<T>() where T : Window =>
         Application.Current.Windows.OfType<T>().First();

        private class WindowInfo
        {
            public IRegionManager RegionManager { get; set; } = null!;
            public string ViewName { get; set; } = null!;
            public Action<NavigationParameters?>? Callback { get; set; }
            public NavigationParameters? Parameters { get; set; }
        }
    }
}
