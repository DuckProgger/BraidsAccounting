using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий сервис взаимодействия с материалами. 
    /// </summary>
    internal interface IItemsService
    {
        Item Add(Item item);

        /// <summary>
        /// Определяет содержится ли выбранный производитель в каталоге.
        /// </summary>
        /// <param name="manufacturerName">Название производителя.</param>
        /// <returns></returns>
        bool ContainsManufacturer(string manufacturerName);
        void Edit(Item item);
        Item? GetItem(string manufacturer, string article, string color);
        IEnumerable<Item> GetItems(bool onlyInStock);
    }
}