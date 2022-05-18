using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

/// <summary>
/// Интерфейс, представляющий сервис взаимодействия с перечнем выполненных работ. 
/// </summary>
internal interface IServiceProvider : IEntityService<Service>
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
    Task AddAsync(Service service);
    Task<List<ServiceProfits>> GetProfitsAsync(FilterOptions options);
    Task<decimal> GetTotalNetProfitAsync(FilterOptions options);
}
