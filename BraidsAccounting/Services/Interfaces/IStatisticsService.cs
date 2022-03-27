using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IStatisticsService
    {
        public IEnumerable<WastedItemForm> GetWastedItemForms(StatisticsFilterOptions options);
    }
}