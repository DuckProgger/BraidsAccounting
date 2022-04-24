using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Exceptions;
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
    /// Реализация сервиса <see cref = "ICatalogueService" />.
    /// </summary>
    internal class CatalogueService : ICatalogueService
    {
        private readonly IRepository<Item> catalogue;

        public CatalogueService(IRepository<Item> catalogue)
        {
            this.catalogue = catalogue;
        }

        public bool ContainsManufacturer(string manufacturerName) =>
            catalogue.Items.Any(i => i.Manufacturer.Name == manufacturerName);

        public async Task<List<Item>> GetAllAsync(bool onlyInStock)
        {
            if (onlyInStock) return await GetOnlyInStockCatalogue();
            return await GetCatalogue();
        }

        private async Task<List<Item>> GetCatalogue() =>
          await catalogue.Items.ToListAsync();

        private async Task<List<Item>> GetOnlyInStockCatalogue() =>
         await catalogue.Items
                .Include(i => i.StoreItems)
                .Where(i => i.StoreItems.Any(si => si.ItemId == i.Id))
                .ToListAsync();

        public async Task<Item?> GetAsync(string manufacturer, string article, string color) =>
               await catalogue.Items.FirstOrDefaultAsync(i =>
                i.Manufacturer.Name == manufacturer
                && i.Article == article
                && i.Color == color);

        public async Task<Item> AddAsync(Item item)
        {
            var manufacturersService = ServiceLocator.GetService<IManufacturersService>();
            Manufacturer? manufacturer = await manufacturersService.GetAsync(item.Manufacturer.Name);
            if (manufacturer == null) throw new ArgumentNullException(nameof(manufacturer), "Производитель не найден.");
            item.Manufacturer = manufacturer;
            return await catalogue.CreateAsync(item);
        }

        public async Task EditAsync(Item item) => await catalogue.EditAsync(item);

        public async Task RemoveAsync(Item item)
        {
            // Проверяем наличие материала на складе и 
            // использование материала в качестве израсходованного
            var storeService = ServiceLocator.GetService<IStoreService>();
            bool existsInStore = storeService.GetItem(item.Manufacturer.Name, item.Article, item.Color) != null;
            if (existsInStore) throw new ArgumentException("Материал используется на складе.");
            var wastedItemsService = ServiceLocator.GetService<IWastedItemsService>();
            bool existsInWasteditems = wastedItemsService.GetItem(item.Manufacturer.Name, item.Article, item.Color) != null;
            if (existsInWasteditems) throw new ArgumentException("Материал используется в качестве израходованного материала.");

            // Удалить материал из каталога
            await catalogue.RemoveAsync(item.Id);
        }

    }
}
