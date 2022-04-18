using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure
{
    /// <summary>
    /// Класс для отображения информационной строки в представлении.
    /// </summary>
    public class MessageProvider : INotifyPropertyChanged
    {
        private string? _message;
        private readonly bool disappearing;
        private readonly TimeSpan disappearingDelay;
        private const double defaultDisappearingDelay = 3.0;

        /// <summary>
        /// Создаёт экземпляр <see cref = "MessageProvider" />.
        /// </summary>
        /// <param name="disappearing">Определяет требуется ли скрывать сообщение по 
        /// истечении 3 сек.</param>
        public MessageProvider(bool disappearing = false)
        {
            this.disappearing = disappearing;
            disappearingDelay = TimeSpan.FromSeconds(defaultDisappearingDelay);
        }

        /// <summary>
        /// Создаёт экземпляр <see cref = "MessageProvider" />.
        /// </summary>
        /// <param name="disappearingDelay">Время задержки исчезания сообщения.</param>
        /// <param name="disappearing">Определяет требуется ли скрывать сообщение по 
        /// истечении времени, заданного в disappearingDelay.</param>
        public MessageProvider(TimeSpan disappearingDelay, bool disappearing = false)
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
                _message = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMessage));
                if (disappearing)
                    ClearMessage();
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
        private async void ClearMessage()
        {
            await Task.Delay(disappearingDelay);
            Message = string.Empty;
        }
    }
}