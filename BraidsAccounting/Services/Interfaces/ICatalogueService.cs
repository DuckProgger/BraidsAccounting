using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

/// <summary>
/// Интерфейс, представляющий сервис взаимодействия с материалами. 
/// </summary>
internal interface ICatalogueService : IEntityService<Item>
{
    Task<Item> AddAsync(Item item);

    /// <summary>
    /// Определяет содержится ли выбранный производитель в каталоге.
    /// </summary>
    /// <param name="id">Id производителя.</param>
    /// <returns></returns>
    bool ContainsManufacturer(int id);
    Task EditAsync(Item item);
    Task<Item?> GetAsync(string manufacturer, string article, string color);
    Task<List<Item>> GetAllAsync(bool onlyInStock);
    Task RemoveAsync(Item item);
    Task<bool> Contains(Item item);
}
