using System;
using System.Windows;

namespace BraidsAccounting.Services.Interfaces
{ 
    /// <summary>
  /// Интерфейс, представляющий сервис взаимодействия с окнами. 
  /// </summary>
    internal interface IViewService
    {
        /// <summary>
        /// Открыть окно типа TWindowType.
        /// </summary>
        /// <typeparam name="TWindowType"></typeparam>
        void ActivateWindow<TWindowType>() where TWindowType : Window, new();
        /// <summary>
        /// Открыть окно типа <typeparamref name = "TOpenedWindow"/> с последующим закрытием.
        /// При этом окно типа <typeparamref name = "TClosedWindow"/> будет закрыто и
        /// откроется после закрытия окна <typeparamref name = "TOpenedWindow"/> с выполнением
        /// действия, содержащегося в <paramref name = "action"/>.
        /// </summary>
        /// <typeparam name="TOpenedWindow">Тип открываемого окна.</typeparam>
        /// <typeparam name="TClosedWindow">Тип закрываемого окна.</typeparam>
        /// <param name="action"></param>
        void ActivateWindowWithClosing<TOpenedWindow, TClosedWindow>(Action? action = null)
              where TOpenedWindow : Window, new()
              where TClosedWindow : Window, new();
        /// <summary>
        /// Получить окно типа <typeparamref name = "T"/>.
        /// </summary>
        /// <typeparam name="T">Тип получаемого окна.</typeparam>
        /// <returns></returns>
        T GetWindow<T>() where T : Window;
    }
}