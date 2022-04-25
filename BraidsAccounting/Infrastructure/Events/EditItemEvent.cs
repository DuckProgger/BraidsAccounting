using BraidsAccounting.DAL.Entities;
using Prism.Events;

namespace BraidsAccounting.Infrastructure.Events
{
    /// <summary>
    /// Класс для публикации и подписки на событие редактирования материала в каталоге.
    /// </summary>
    internal class EditItemEvent : PubSubEvent<Item>
    {
    }
}
