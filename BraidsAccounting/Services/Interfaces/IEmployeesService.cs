using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

internal interface IEmployeesService : IEntityService<Employee>
{
    /// <summary>
    /// Добавить нового сотрудника.
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    Task AddAsync(Employee employee);
    /// <summary>
    /// Редактировать существующего сотрудника.
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    Task EditAsync(Employee employee);
    Task<Employee?> GetAsync(int id);

    /// <summary>
    /// Получить список всех сотрудников.
    /// </summary>
    /// <returns></returns>
    Task<List<Employee>> GetAllAsync();
}
