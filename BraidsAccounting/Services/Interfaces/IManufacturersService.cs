using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IManufacturersService
    {
        void AddManufacturer(Manufacturer manufacturer);
        void EditManufacturer(Manufacturer manufacturer);
        IEnumerable<string> GetManufacturerNames();
        IEnumerable<Manufacturer> GetManufacturers();
        void RemoveManufacturer(int id);
    }
}