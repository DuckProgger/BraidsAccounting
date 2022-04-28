using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure
{
    /// <summary>
    /// Класс для отображения информационной строки в представлении.
    /// </summary>
    public class Notifier : INotifyPropertyChanged
    {
        private string? _message;
        private readonly bool disappearing;
        private readonly TimeSpan disappearingDelay;
        private const double defaultDisappearingDelay = 3.0;
        private CancellationToken ct;
        CancellationTokenSource cts;

        /// <summary>
        /// Создаёт экземпляр <see cref = "Notifier" />.
        /// </summary>
        /// <param name="disappearing">Определяет требуется ли скрывать сообщение по 
        /// истечении 3 сек.</param>
        public Notifier(bool disappearing = false)
        {
            this.disappearing = disappearing;
            disappearingDelay = TimeSpan.FromSeconds(defaultDisappearingDelay);
            cts = new CancellationTokenSource();
            ct = cts.Token;
        }

        /// <summary>
        /// Создаёт экземпляр <see cref = "Notifier" />.
        /// </summary>
        /// <param name="disappearingDelay">Время задержки исчезания сообщения.</param>
        /// <param name="disappearing">Определяет требуется ли скрывать сообщение по 
        /// истечении времени, заданного в disappearingDelay.</param>
        public Notifier(TimeSpan disappearingDelay, bool disappearing = false)
        {
            this.disappearing = disappearing;
            this.disappearingDelay = disappearingDelay;
        }

        /// <summary>
        /// Сообщение, которое нужно вывести на экран.
        /// </summary>
        public string? Message
        {
            get => _message;
            set
            {
                if (disappearing)
                {
                    if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(_message)/*&& !value.Equals(_message)*/)
                        RestartMessage();
                    else
                        ClearMessage(ct);
                }
                _message = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMessage));
            }
        }

        /// <summary>
        /// Флаг наличия сообщения.
        /// </summary>
        public bool HasMessage => !string.IsNullOrEmpty(Message);

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        /// <summary>
        /// Очистить сообщение.
        /// </summary>
        private async void ClearMessage(CancellationToken ct)
        {
            var delayTask = Task.Delay(disappearingDelay, ct);
            await delayTask.ContinueWith((o) => Message = string.Empty);
        }

        private void RestartMessage()
        {
            cts.Cancel();
            cts = new();
            ClearMessage(cts.Token);
        }
    }
}