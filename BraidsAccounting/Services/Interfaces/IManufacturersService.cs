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
        void AddManufacturer(Manufacturer? manufacturer);
        /// <summary>
        /// Изменить производителя материала из каталога.
        /// </summary>
        /// <param name="manufacturer">Название производителя.</param>
        void EditManufacturer(Manufacturer? manufacturer);
        /// <summary>
        /// Получить объект производителя материалов.
        /// </summary>
        /// <param name="name">Имя производителя.</param>
        /// <returns></returns>
        Manufacturer? GetManufacturer(string name);
        /// <summary>
        /// Получить список названий производителей материалов.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetManufacturerNamesAsync();
        /// <summary>
        /// Получить список производителей материалов.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Manufacturer> GetManufacturers();
        /// <summary>
        /// Удалить выбранного производителя.
        /// </summary>
        /// <param name="id">ID производителя.</param>
        void RemoveManufacturer(int id);
    }
}