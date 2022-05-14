using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

/// <summary>
/// Интерфейс, представляющий сервис статистики. 
/// </summary>
internal interface IWastedItemsService
{
    /// <summary>
    /// Получить список израсходованных материалов на основании фильтров, установленных в <paramref name = "options" />.
    /// </summary>
    /// <param name="options">Параметры фильтрации.</param>
    /// <returns></returns>
    Task<List<WastedItemForm>> GetWastedItemFormsAsync(WastedItemsFilterOptions options);
    /// <summary>
    /// Получить общую сумму расходов для коллекции израсходованных материалов.
    /// </summary>
    /// <param name="itemForms"></param>
    /// <returns></returns>
    Task<decimal> GetTotalExpensesAsync(WastedItemsFilterOptions options);
    WastedItem? GetItem(string manufacturer, string article, string color);
    Task AddRangeAsync(IEnumerable<WastedItem> items);
    Task<bool> ContainsItemAsync(int itemId);
}
