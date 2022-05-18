using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

/// <summary>
/// Интерфейс, представляющий сервис взаимодействия со складом. 
/// </summary>
internal interface IStoreService : IEntityService<StoreItem>
{
    /// <summary>
    /// Добавить материал на склад.
    /// </summary>
    /// <param name="storeItem">Добавляемый материал.</param>
    Task AddAsync(StoreItem? storeItem);
    /// <summary>
    /// Редактировать материал со склада.
    /// </summary>
    /// <param name="storeItem">Редактируемый материал.</param>
    Task EditAsync(StoreItem? storeItem);
    /// <summary>
    /// Получить список материалов на складе.
    /// </summary>
    /// <returns></returns>
    Task<List<StoreItem>> GetItemsAsync();

    /// <summary>
    /// Удалить материал со склада.
    /// </summary>
    /// <param name="id">ID материала.</param>
    Task RemoveAsync(int id);
    /// <summary>
    /// Удалить список материалов со склада.
    /// </summary>
    /// <param name="items">Материалы, которые нужно удалить со склада.</param>
    Task RemoveItemsAsync(IEnumerable<WastedItem?> wastedItems);

    StoreItem? Get(string manufacturer, string article, string color);
    /// <summary>
    /// Получить количество товара на складе.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>ID материала.</returns>
    int Count(string manufacturer, string article, string color);
    Task<bool> ContainsItemAsync(int itemId);
    Task<StoreItem?> GetAsync(int id);
    Task<StoreItem?> GetByItemAsync(int itemId);
}