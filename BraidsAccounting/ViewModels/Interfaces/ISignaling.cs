using BraidsAccounting.Infrastructure;

namespace BraidsAccounting.ViewModels.Interfaces
{
    internal interface ISignaling
    {
        /// <summary>
        /// Выводимое сообщение о статусе.
        /// </summary>
        MessageProvider StatusMessage { get; }
        /// <summary>
        /// Выводимое сообщение об ошибке.
        /// </summary>
        MessageProvider ErrorMessage { get; }
        /// <summary>
        /// Выводимое сообщение о предупреждении.
        /// </summary>
        public MessageProvider WarningMessage { get; }
    }
}
