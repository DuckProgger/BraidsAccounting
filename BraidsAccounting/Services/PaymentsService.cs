using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
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

    public PaymentsService(
        IRepository<Payment> paymentsRepository
        , IWastedItemsService statisticsService
        )
    {
        this.paymentsRepository = paymentsRepository;
        this.statisticsService = statisticsService;
    }

    public async Task AddAsync(Payment payment) =>
      await paymentsRepository.CreateAsync(payment);

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
        WastedItemsFilterOptions options = new()
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
}
