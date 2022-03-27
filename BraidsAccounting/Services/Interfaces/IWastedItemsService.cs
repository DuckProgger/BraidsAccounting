using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IWastedItemsService
    {
        public IEnumerable<WastedItemForm> GetWastedItemForms(string? workerName, bool grouping, DatePeriod period);
    }
}