using BraidsAccounting.Models;

namespace BraidsAccounting.Services
{
    internal class StatisticsFilterOptions
    {
        public bool EnableWorkerFilter { get; set; } = false;
        public bool EnablePeriodFilter { get; set; } = false;
        /// <summary>
        /// Флаг группировки потраченных материалов.
        /// </summary>
        public bool EnableGrouping { get; set; } = false;
        /// <summary>
        /// Фильтр по имени сотрудника.
        /// </summary>
        public string? WorkerNameFilter { get; set; }
        public DatePeriod DatePeriod { get; set; }
    }
}
