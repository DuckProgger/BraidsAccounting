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
        Task AddAsync(Manufacturer? manufacturer);
        /// <summary>
        /// Изменить производителя материала из каталога.
        /// </summary>
        /// <param name="manufacturer">Название производителя.</param>
        Task EditAsync(Manufacturer? manufacturer);
        /// <summary>
        /// Получить объект производителя материалов.
        /// </summary>
        /// <param name="name">Имя производителя.</param>
        /// <returns></returns>
        Task<Manufacturer?> GetAsync(string name);
        /// <summary>
        /// Получить список названий производителей материалов.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetNamesAsync();
        /// <summary>
        /// Получить список производителей материалов.
        /// </summary>
        /// <returns></returns>
        Task<List<Manufacturer>> GetAllAsync();
        /// <summary>
        /// Удалить выбранного производителя.
        /// </summary>
        /// <param name="id">ID производителя.</param>
        Task RemoveAsync(int id);
    }
}