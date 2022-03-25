using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IWastedItemsService
    {
        IEnumerable<WastedItemForm> GetWastedItemForms(string? workerName, bool grouping);
    }
}