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

        public async Task<List<Manufacturer>> GetManufacturersAsync() => await manufacturers.Items.ToListAsync();
        public async Task<List<string>> GetManufacturerNamesAsync() => await manufacturers.Items.Select(m => m.Name).ToListAsync();
        public async Task<Manufacturer?> GetManufacturerAsync(string name) => await manufacturers.Items.FirstOrDefaultAsync(m => m.Name == name);

        public async Task AddManufacturerAsync(Manufacturer? manufacturer)
        {
            if (manufacturer is null) throw new ArgumentNullException(nameof(manufacturer));
            if (string.IsNullOrEmpty(manufacturer.Name) || manufacturer.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(manufacturer.Name));
            await manufacturers.CreateAsync(manufacturer);
        }

        public async Task EditManufacturerAsync(Manufacturer? manufacturer)
        {
            if (manufacturer is null) throw new ArgumentNullException(nameof(manufacturer));
            if (string.IsNullOrEmpty(manufacturer.Name) || manufacturer.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(manufacturer.Name));
            await manufacturers.EditAsync(manufacturer);
        }

        public async Task RemoveManufacturerAsync(int id) => await manufacturers.RemoveAsync(id);
    }
}
