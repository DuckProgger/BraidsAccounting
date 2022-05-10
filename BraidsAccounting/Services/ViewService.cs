using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BraidsAccounting.Services;

/// <summary>
/// Реализация сервиса <see cref = "IViewService" />.
/// </summary>
internal class ViewService : IViewService
{
    private readonly LinkedList<ViewContext> callContext = new();

    public void ShowPopupWindow(string viewName, NavigationParameters? parameters = null, Action<NavigationParameters?>? callback = null)
    {
        if (string.IsNullOrEmpty(viewName))
            throw new ArgumentNullException(nameof(viewName), "Название переключаемого представления не может быть пустым.");
        if (callContext.Count == 0)
        {
            CreatePopupWindow(viewName, parameters, callback);
            return;
        }
        ViewContext? currentWindowInfo = callContext.Last();
        ViewContext newWindowInfo = new()
        {
            RegionManager = currentWindowInfo.RegionManager,
            ViewName = viewName,
        };
        callContext.AddLast(newWindowInfo);
        newWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
    }

    public void GoBack()
    {
        ViewContext? currentLayer = callContext.Last();
        callContext.RemoveLast();
        currentLayer.Callback?.Invoke(currentLayer.Parameters);
        if (callContext.Count == 0)
        {
            ClosePopupWindow();
            return;
        }
        string? prevViewName = callContext.Last().ViewName;
        currentLayer.RegionManager.RequestNavigate(RegionNames.Popup, prevViewName, currentLayer.Parameters);
    }
    public void AddParameter(string name, object value)
    {
        var lastLayer = callContext.Last();
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
        popupWindow.Closed += (s, e) => callContext.Clear();
        mainWindow.Hide();
        popupWindow.Show();
        IRegionManager? regionManager = ServiceLocator.GetService<IRegionManager>();
        IRegionManager? scopedRegionManager = regionManager.CreateRegionManager();
        RegionManager.SetRegionManager(popupWindow, scopedRegionManager);
        ViewContext? popupWindowInfo = new ViewContext()
        {
            RegionManager = scopedRegionManager,
            ViewName = viewName,
            Callback = callback
        };
        callContext.AddLast(popupWindowInfo);
        popupWindowInfo.RegionManager.RequestNavigate(RegionNames.Popup, viewName, parameters);
    }

    private static T GetWindow<T>() where T : Window =>
     Application.Current.Windows.OfType<T>().First();

    private class ViewContext
    {
        public IRegionManager RegionManager { get; set; } = null!;
        public string ViewName { get; set; } = null!;
        public Action<NavigationParameters?>? Callback { get; set; }
        public NavigationParameters? Parameters { get; set; }
    }
}
