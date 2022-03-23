using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class ManufacturersService : IManufacturersService
    {
        private readonly IRepository<Manufacturer> manufacturers;

        public ManufacturersService(IRepository<Manufacturer> manufacturers)
        {
            this.manufacturers = manufacturers;
        }

        public IEnumerable<Manufacturer> GetManufacturers() => manufacturers.Items;
        public IEnumerable<string> GetManufacturerNames() => manufacturers.Items.Select(m => m.Name);

        public void AddManufacturer(Manufacturer manufacturer)
        {
            manufacturers.Create(manufacturer);
        }

        public void EditManufacturer(Manufacturer manufacturer)
        {
            manufacturers.Edit(manufacturer);
        }

        public void RemoveManufacturer(int id)
        {
            manufacturers.Remove(id);
        }
    }
}
