using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

internal class ServiceProvider : Interfaces.IServiceProvider, IHistoryTracer<Service>
{
    private readonly IRepository<Service> services;
    private readonly IStoreService store;
    private readonly IRepository<WastedItem> wastedItemsService;
    private readonly IEmployeesService employeesService;
    private readonly IHistoryService historyService;

    public ServiceProvider(
        IRepository<Service> services
        , IStoreService store
        , IRepository<WastedItem> wastedItemsService
        , IEmployeesService employeesService
        , IHistoryService historyService
        )
    {
        this.services = services;
        this.store = store;
        this.wastedItemsService = wastedItemsService;
        this.employeesService = employeesService;
        this.historyService = historyService;
    }

    public async Task AddAsync(Service service)
    {
        // Найти ID сотрудника в БД
        service.Employee = await employeesService.GetAsync(service.Employee.Name)
            ?? throw new ArgumentException("Сотрудник не найден.", nameof(service.Employee));

        // Добавить услугу в БД
        CalculateNetProfit(service);
        var newService = await services.CreateAsync(service);
        await historyService.WriteCreateOperationAsync(service.GetEtityData(this));

        BindWastedItemsToService(newService);
        wastedItemsService.CreateRange(newService.WastedItems);

        // Убрать использованные товары со склада
        await store.RemoveItemsAsync(service.WastedItems);
    }

    /// <summary>
    /// Получить все имена сотрудников, которые когда-либо 
    /// выполняли работу
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetNamesAsync()
    {
        return await services.Items.Select(s => s.Employee.Name).Distinct().ToListAsync();
    }

    private static void BindWastedItemsToService(Service s)
    {
        foreach (var wastedItem in s.WastedItems)
        {
            if (wastedItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(wastedItem.Count));
            wastedItem.ServiceId = s.Id;
        }
    }

    private static void CalculateNetProfit(Service service)
    {
        decimal expenses = 0;
        foreach (var wastedItem in service.WastedItems)
            expenses += wastedItem.Item.Manufacturer.Price * wastedItem.Count;
        service.NetProfit = service.Profit - expenses;
    }

    IEntityDataBuilder<Service> IHistoryTracer<Service>.ConfigureEntityData(IEntityDataBuilder<Service> builder, Service entity) =>
        builder.AddInfo(s => s.Employee.Name, entity.Employee.Name)
        .AddInfo(s => s.Profit, entity.Profit)
        .AddInfo(s => s.NetProfit, entity.NetProfit)
        .AddInfo(s => s.DateTime, entity.DateTime);
}
