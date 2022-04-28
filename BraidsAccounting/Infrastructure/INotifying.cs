namespace BraidsAccounting.Infrastructure
{
    internal interface INotifying
    {
        /// <summary>
        /// Выводимое сообщение о статусе.
        /// </summary>
        Notifier Status { get; }
        /// <summary>
        /// Выводимое сообщение об ошибке.
        /// </summary>
        Notifier Error { get; }
        /// <summary>
        /// Выводимое сообщение о предупреждении.
        /// </summary>
        public Notifier Warning { get; }
    }
}
