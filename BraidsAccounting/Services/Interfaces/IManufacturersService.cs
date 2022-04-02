using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий сервис взаимодействия с производителями материалов. 
    /// </summary>
    internal interface IManufacturersService
    {
        /// <summary>
        /// Добавить производителя материала в каталог.
        /// </summary>
        /// <param name="manufacturer">Название производителя.</param>
        Task AddManufacturerAsync(Manufacturer? manufacturer);
        /// <summary>
        /// Изменить производителя материала из каталога.
        /// </summary>
        /// <param name="manufacturer">Название производителя.</param>
        Task EditManufacturerAsync(Manufacturer? manufacturer);
        /// <summary>
        /// Получить объект производителя материалов.
        /// </summary>
        /// <param name="name">Имя производителя.</param>
        /// <returns></returns>
        Task<Manufacturer?> GetManufacturerAsync(string name);
        /// <summary>
        /// Получить список названий производителей материалов.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetManufacturerNamesAsync();
        /// <summary>
        /// Получить список производителей материалов.
        /// </summary>
        /// <returns></returns>
        Task<List<Manufacturer>> GetManufacturersAsync();
        /// <summary>
        /// Удалить выбранного производителя.
        /// </summary>
        /// <param name="id">ID производителя.</param>
        Task RemoveManufacturerAsync(int id);
    }
}