using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IItemsService
    {
        //IEnumerable<string> GetManufacturers();
        bool ContainsManufacturer(string manufacturerName);
    }
}