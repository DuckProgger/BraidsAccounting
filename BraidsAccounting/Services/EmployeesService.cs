using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class EmployeesService : IEmployeesService
    {
        private readonly IRepository<Employee> employees;

        public EmployeesService(IRepository<Employee> employees)
        {
            this.employees = employees;
        }

        public async Task AddAsync(Employee employee) =>
            await employees.CreateAsync(employee);

        public async Task EditAsync(Employee employee) => 
            await employees.EditAsync(employee);

        public async Task<List<Employee>> GetAllAsync() =>
            await employees.Items.ToListAsync();

        public async Task<Employee?> GetAsync(string name) =>
            await employees.Items.FirstOrDefaultAsync(e => e.Name.Equals(name));
    }
}
