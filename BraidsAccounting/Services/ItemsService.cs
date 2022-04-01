using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Item> GetItems(bool onlyInStock)
        {
            if (onlyInStock) return GetOnlyInStockCatalogue();
            return GetCatalogue();
        }

        private IEnumerable<Item> GetCatalogue() =>
            catalogue.Items;

        private IEnumerable<Item> GetOnlyInStockCatalogue() =>
          catalogue.Items
                .Include(i => i.StoreItems)
                .Where(i => i.StoreItems.Any(si => si.ItemId == i.Id));

        public Item? GetItem(string manufacturer, string article, string color) =>
                catalogue.Items.FirstOrDefault(i =>
                i.Manufacturer.Name == manufacturer
                && i.Article == article
                && i.Color == color);

        public Item Add(Item item) =>
            catalogue.Create(item);

        public void Edit(Item item) => catalogue.Edit(item);

    }
}
