using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Реализация сервиса <see cref = "IManufacturersService" />.
    /// </summary>
    internal class ManufacturersService : IManufacturersService
    {
        private readonly IRepository<Manufacturer> manufacturers;

        public ManufacturersService(IRepository<Manufacturer> manufacturers)
        {
            this.manufacturers = manufacturers;
        }

        public IEnumerable<Manufacturer> GetManufacturers() => manufacturers.Items;
        public async Task<List<string>> GetManufacturerNamesAsync() => await manufacturers.Items.Select(m => m.Name).ToListAsync();
        public Manufacturer? GetManufacturer(string name) => manufacturers.Items.FirstOrDefault(m => m.Name == name);

        public void AddManufacturer(Manufacturer? manufacturer)
        {
            if (manufacturer is null) throw new ArgumentNullException(nameof(manufacturer));
            if (string.IsNullOrEmpty(manufacturer.Name) || manufacturer.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(manufacturer.Name));
            manufacturers.Create(manufacturer);
        }

        public void EditManufacturer(Manufacturer? manufacturer)
        {
            if (manufacturer is null) throw new ArgumentNullException(nameof(manufacturer));
            if (string.IsNullOrEmpty(manufacturer.Name) || manufacturer.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(manufacturer.Name));
            manufacturers.Edit(manufacturer);
        }

        public void RemoveManufacturer(int id) => manufacturers.Remove(id);
    }
}
