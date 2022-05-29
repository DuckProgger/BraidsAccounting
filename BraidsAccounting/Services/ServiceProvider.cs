using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

internal class ServiceProvider : Interfaces.IServiceProvider
{
    private readonly IRepository<Service> services;
    private IStoreService store;
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
        service.Employee = await employeesService.GetAsync(service.Employee.Id)
            ?? throw new ArgumentException(Resources.EmployeeNotFound, nameof(service.Employee));

        // Добавить услугу в БД
        CalculateNetProfit(service);
        var newService = await services.CreateAsync(service);
        await historyService.WriteCreateOperationAsync(service.GetEtityData(this));

        GetActualPricesForWastedItems(newService.WastedItems);
        BindWastedItemsToService(newService);
        wastedItemsService.CreateRange(newService.WastedItems);

        // Убрать использованные товары со склада
        store = ServiceLocator.GetService<IStoreService>(); // обновить контекст (значения могли измениться)
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

    private static void GetActualPricesForWastedItems(IEnumerable<WastedItem> wastedItems)
    {
        var manufacturersService = ServiceLocator.GetService<IManufacturersService>();
        foreach (var wastedItem in wastedItems)
            wastedItem.Price = manufacturersService.GetPrice(wastedItem.Item.Manufacturer.Name);
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
            expenses += wastedItem.Price * wastedItem.Count;
        service.NetProfit = service.Profit - expenses;
    }

    public async Task<List<ServiceProfits>> GetProfitsAsync(FilterOptions options)
    {
        var baseQuery = GetFilteredQuery(options);
        var totalQuery = AddSelect(baseQuery);
        return await totalQuery.ToListAsync();
    }

    public async Task<decimal> GetTotalNetProfitAsync(FilterOptions options)
    {
        var baseQuery = GetFilteredQuery(options);
        return await baseQuery.SumAsync(w => w.NetProfit);
    }

    private IQueryable<Service> GetFilteredQuery(FilterOptions options)
    {
        IQueryable<Service>? baseQuery = services.Items;
        if (options.EnableWorkerFilter) AddWorkerFilter(ref baseQuery, options.WorkerNameFilter);
        if (options.EnablePeriodFilter) AddPeriodFilter(ref baseQuery, options.DatePeriod);
        return baseQuery;
    }

    /// <summary>
    /// Добавляет к базовому запросу фильтр работника.
    /// </summary>
    /// <param name="query">Запрос.</param>
    /// <param name="workerName">Имя работника, по которому нужно применить фильтр.</param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void AddWorkerFilter(ref IQueryable<Service> query, string? workerName)
    {
        if (workerName is null) throw new ArgumentNullException(nameof(workerName));
        query = query.Where(w => w.Employee.Name == workerName);
    }

    /// <summary>
    /// Добавляет к базовому запросу фильтр интервала даты.
    /// </summary>
    /// <param name="query">Запрос.</param>
    /// <param name="period">Интервал дат, в пределах которых выводится результат.</param>
    private static void AddPeriodFilter(ref IQueryable<Service> query, DatePeriod period)
    {
        query = query.Where(w =>
        w.DateTime.Date >= period.Start
        && w.DateTime.Date <= period.End
        );
    }

    /// <summary>
    /// Добавляет к запросу выборку.
    /// </summary>
    /// <param name="query">Запрос.</param>
    /// <returns></returns>
    private static IQueryable<ServiceProfits> AddSelect(IQueryable<Service> query)
    {
        return query.Select(s => new ServiceProfits()
        {
            Profit = s.Profit,
            NewProfit = s.NetProfit,
            Date = s.DateTime.Date
        });
    }

    IEntityDataBuilder<Service> IHistoryTracer<Service>.ConfigureEntityData(IEntityDataBuilder<Service> builder, Service entity) =>
        builder.AddInfo(s => s.Employee.Name, entity.Employee.Name)
        .AddInfo(s => s.Profit, entity.Profit)
        .AddInfo(s => s.NetProfit, entity.NetProfit)
        .AddInfo(s => s.DateTime, entity.DateTime);

    public bool Validate(Service entity, out IEnumerable<string> errorMessages)
    {
        List<string> errorMessagesList = new();
        errorMessages = errorMessagesList;
        bool haveError = false;
        if (entity.Employee is null || string.IsNullOrWhiteSpace(entity.Employee.Name))
        {
            errorMessagesList.Add(Resources.EmployeeNotSelected);
            haveError = true;
        }
        if (entity.Profit < 0)
        {
            errorMessagesList.Add(Resources.InvalidServiceProfit);
            haveError = true;
        }
        if (entity.WastedItems is not null && entity.WastedItems.Count > 0)
            foreach (var wastedItem in entity.WastedItems)
                if (wastedItem.Count == 0)
                {
                    errorMessagesList.Add(Resources.WastedItemInvalidCount);
                    haveError = true;
                    break;
                }
        return !haveError;
    }
}
