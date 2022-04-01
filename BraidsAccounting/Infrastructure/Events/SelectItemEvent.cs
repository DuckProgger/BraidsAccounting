using BraidsAccounting.DAL.Entities;
using Prism.Events;

namespace BraidsAccounting.Infrastructure.Events
{
    /// <summary>
    /// Класс для публикации и подписки на событие выбора позиции на складе.
    /// </summary>
    internal class SelectItemEvent : PubSubEvent<Item>
    {
    }
}
