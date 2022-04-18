using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IEmployeesService
    {
        /// <summary>
        /// Добавить нового сотрудника.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task AddEmployeeAsync(Employee employee);
        /// <summary>
        /// Редактировать существующего сотрудника.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task EditEmployeeAsync(Employee employee);
        Task<Employee?> GetEmployeeAsync(string name);

        /// <summary>
        /// Получить список всех сотрудников.
        /// </summary>
        /// <returns></returns>
        Task<List<Employee>> GetEmployeesAsync();
    }
}