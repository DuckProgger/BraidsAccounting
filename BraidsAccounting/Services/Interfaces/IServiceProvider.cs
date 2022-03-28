using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;

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
        IEnumerable<string> GetNames();
        /// <summary>
        /// Добавить выполненную работу.
        /// </summary>
        /// <param name="service"></param>
        void ProvideService(Service service);
    }
}