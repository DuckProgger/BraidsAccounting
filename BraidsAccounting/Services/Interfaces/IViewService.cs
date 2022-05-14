using Prism.Regions;
using System;
using System.Collections.Generic;
using static BraidsAccounting.Services.ViewService;

namespace BraidsAccounting.Services.Interfaces;

/// <summary>
/// Интерфейс, представляющий сервис взаимодействия с окнами. 
/// </summary>
internal interface IViewService
{
    void ShowPopupWindow(string viewName, NavigationParameters? parameters = null, Action<NavigationParameters?>? callback = null);
    void GoBack();
    void AddParameter(string name, object value);

    LinkedList<ViewContext> CallContext { get; }
}
