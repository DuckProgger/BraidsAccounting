using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

internal class PaymentsService : IPaymentsService
{
    private readonly IRepository<Payment> paymentsRepository;
    private readonly IWastedItemsService statisticsService;
    private readonly IHistoryService historyService;

    public PaymentsService(
        IRepository<Payment> paymentsRepository
        , IWastedItemsService statisticsService
        , IHistoryService historyService
        )
    {
        this.paymentsRepository = paymentsRepository;
        this.statisticsService = statisticsService;
        this.historyService = historyService;
    }

    public async Task AddAsync(Payment payment)
    {
        await paymentsRepository.CreateAsync(payment);
        await historyService.WriteCreateOperationAsync(payment.GetEtityData(this));
    }

    public async Task<List<Payment>> GetRangeAsync(string employeeName) =>
        await GetFilteredQuery(employeeName).ToListAsync();

    public async Task<decimal> GetTotalAmountAsync(string employeeName) =>
        await GetFilteredQuery(employeeName).SumAsync(p => p.Amount);

    private IQueryable<Payment> GetFilteredQuery(string employeeName) =>
        paymentsRepository.Items
           .Include(p => p.Employee)
           .Where(p => p.Employee.Name.Equals(employeeName));

    public async Task<decimal> GetDebtAsync(string employeeName)
    {
        // Получить общую сумму расходов на израсходованные
        // сотрудником материалы за всё время
        FilterOptions options = new()
        {
            EnableGrouping = false,
            EnablePeriodFilter = false,
            EnableWorkerFilter = true,
            WorkerNameFilter = employeeName
        };
        decimal expenses = await statisticsService.GetTotalExpensesAsync(options);

        // Получить общую сумму пополнений сотрудника
        decimal payments = await GetTotalAmountAsync(employeeName);

        // Итоговая задолженность есть разница между
        // суммой расходов и суммой пополнений
        return expenses - payments;
    }

    IEntityDataBuilder<Payment> IHistoryTracer<Payment>.ConfigureEntityData(IEntityDataBuilder<Payment> builder, Payment entity) =>
        builder
        .AddInfo(p => p.Employee.Name, entity.Employee.Name)
        .AddInfo(p => p.Amount, entity.Amount)
        .AddInfo(p => p.DateTime, entity.DateTime);

    public bool Validate(Payment entity, out IEnumerable<string> errorMessages)
    {
        List<string> errorMessagesList = new();
        errorMessages = errorMessagesList;
        bool haveError = false;
        if (entity.Amount <= 0)
        {
            errorMessagesList.Add(Resources.AmountMustBePositive);
            haveError = true;
        }
        if (entity.Employee is null)
        {
            errorMessagesList.Add(Resources.EmployeeNotSelected);
            haveError = true;
        }
        return !haveError;
    }
}
