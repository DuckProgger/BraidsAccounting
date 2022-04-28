using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure
{
    /// <summary>
    /// Класс для отображения информационной строки в представлении.
    /// </summary>
    public class NewNotifier : INotifyPropertyChanged
    {
        private readonly bool disappearing;
        private readonly TimeSpan disappearingDelay;
        private const double defaultDisappearingDelay = 3.0;

        /// <summary>
        /// Создаёт экземпляр <see cref = "Notifier" />.
        /// </summary>
        /// <param name="disappearing">Определяет требуется ли скрывать сообщение по 
        /// истечении 3 сек.</param>
        public NewNotifier(bool disappearing = false)
        {
            this.disappearing = disappearing;
            disappearingDelay = TimeSpan.FromSeconds(defaultDisappearingDelay);
        }

        /// <summary>
        /// Создаёт экземпляр <see cref = "Notifier" />.
        /// </summary>
        /// <param name="disappearingDelay">Время задержки исчезания сообщения.</param>
        /// <param name="disappearing">Определяет требуется ли скрывать сообщение по 
        /// истечении времени, заданного в disappearingDelay.</param>
        public NewNotifier(TimeSpan disappearingDelay, bool disappearing = false)
        {
            this.disappearing = disappearing;
            this.disappearingDelay = disappearingDelay;
        }

        public ObservableCollection<string> Messages { get; } = new();

        /// <summary>
        /// Сообщение, которое нужно вывести на экран.
        /// </summary>
        //public string? Message
        //{
        //    get => _message;
        //    set
        //    {
        //        _message = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(HasMessage));
        //        if (disappearing)
        //            RemoveMessage();
        //    }
        //}

        public void Add(string message)
        {
            Messages.Add(message);
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasMessage));
            if (disappearing)
                AutoRemove(message);
        }

        /// <summary>
        /// Флаг наличия сообщения.
        /// </summary>
        public bool HasMessage => Messages.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        /// <summary>
        /// Очистить сообщение.
        /// </summary>
        private async void AutoRemove(string message)
        {
            await Task.Delay(disappearingDelay);
            Messages.Remove(message);
        }

        public void Remove(string message)
        {
            Messages.Remove(message);
        }

    }
}