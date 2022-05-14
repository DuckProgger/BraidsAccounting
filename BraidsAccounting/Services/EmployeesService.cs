using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

internal class EmployeesService : IEmployeesService, IHistoryTracer<Employee>
{
    private readonly IRepository<Employee> employees;
    private readonly IHistoryService historyService;

    public EmployeesService(IRepository<Employee> employees, IHistoryService historyService)
    {
        this.employees = employees;
        this.historyService = historyService;
    }

    public async Task AddAsync(Employee employee)
    {
        await employees.CreateAsync(employee);
        await historyService.WriteCreateOperationAsync(employee.GetEtityData(this));
    }

    public async Task EditAsync(Employee employee)
    {
        var existingEmployee = await employees.GetAsync(employee.Id) with { };
        await employees.EditAsync(employee);
        await historyService.WriteUpdateOperationAsync(existingEmployee.GetEtityData(this), employee.GetEtityData(this));
    }

    public async Task<List<Employee>> GetAllAsync() =>
        await employees.Items.ToListAsync();

    public async Task<Employee?> GetAsync(int id) =>
        await employees.GetAsync(id);

    IEntityDataBuilder<Employee> IHistoryTracer<Employee>.ConfigureEntityData(IEntityDataBuilder<Employee> builder, Employee entity) =>
        builder.AddInfo(e => e.Name, entity.Name);
}
