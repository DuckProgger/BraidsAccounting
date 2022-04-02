using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий сервис взаимодействия с перечнем выполненных работ. 
    /// </summary>
    internal interface IServiceProvider
    {
        /// <summary>
        /// Получить имена всех сотрудников, когда-либо выполнявших работы.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetNamesAsync();
        /// <summary>
        /// Добавить выполненную работу.
        /// </summary>
        /// <param name="service"></param>
        Task ProvideServiceAsync(Service service);
    }
}