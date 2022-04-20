using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Реализация сервиса <see cref = "IItemsService" />.
    /// </summary>
    internal class ItemsService : IItemsService
    {
        private readonly IRepository<Item> catalogue;

        public ItemsService(IRepository<Item> catalogue)
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

        public async Task<Item> AddAsync(Item item) =>
           await catalogue.CreateAsync(item);

        public async Task EditAsync(Item item) => await catalogue.EditAsync(item);

    }
}
