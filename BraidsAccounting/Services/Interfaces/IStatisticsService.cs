using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий сервис статистики. 
    /// </summary>
    internal interface IStatisticsService
    {
        /// <summary>
        /// Получить список израсходованных материалов на основании фильтров, установленных в <paramref name = "options" />.
        /// </summary>
        /// <param name="options">Параметры фильтрации.</param>
        /// <returns></returns>
        public IEnumerable<WastedItemForm> GetWastedItemForms(StatisticsFilterOptions options);
    }
}