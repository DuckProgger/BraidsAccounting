using Prism.Events;

namespace BraidsAccounting.Infrastructure.Events
{
    /// <summary>
    /// Класс для публикации и подписки на события кнопок при взаимодействии между представлениями.
    /// </summary>
    internal class ActionSuccessEvent : PubSubEvent<bool>
    {
    }
}
