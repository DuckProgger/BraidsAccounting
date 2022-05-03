﻿using BraidsAccounting.Modules;
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
    private readonly LinkedList<WindowInfo> windowsStack = new();

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
    }

    public void GoBack()
    {
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
    }

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
