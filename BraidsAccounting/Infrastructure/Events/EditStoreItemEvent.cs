using BraidsAccounting.DAL.Entities;
using Prism.Events;

namespace BraidsAccounting.Infrastructure.Events
{
    /// <summary>
    /// Класс для публикации и подписки на событие редактирования позиции на складе.
    /// </summary>
    internal class EditStoreItemEvent : PubSubEvent<StoreItem>
    {
    }
}
