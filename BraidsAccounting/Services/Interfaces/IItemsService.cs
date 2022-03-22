using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IItemsService
    {
        IEnumerable<string> GetManufacturers();
    }
}