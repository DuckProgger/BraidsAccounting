using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Exceptions;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

internal class EmployeesService : IEmployeesService
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
        // Контроль дубликата
        if (await Contains(employee)) throw new DublicateException(Messages.DublicateEmployee);
        await employees.CreateAsync(employee);
        await historyService.WriteCreateOperationAsync(employee.GetEtityData(this));
    }

    public async Task<bool> Contains(Employee item) =>
        await employees.Items.AnyAsync(i => i.Name == item.Name);

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

    public bool Validate(Employee entity, out IEnumerable<string> errorMessages)
    {
        List<string> errorMessagesList = new();
        errorMessages = errorMessagesList;
        bool haveError = false;
        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            errorMessagesList.Add(Messages.EmployeeNameNotFilled);
            haveError = true;
        }
        return !haveError;
    }

}
