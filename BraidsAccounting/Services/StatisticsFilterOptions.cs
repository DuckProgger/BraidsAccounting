using BraidsAccounting.Models;

namespace BraidsAccounting.Services
{
    internal class StatisticsFilterOptions
    {
        public bool EnableWorkerFilter { get; set; } = false;
        public bool EnablePeriodFilter { get; set; } = false;
        public bool EnableGrouping { get; set; } = false;
        public string? WorkerNameFilter { get; set; }
        public DatePeriod DatePeriod { get; set; }
    }
}
