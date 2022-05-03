using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

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

    public bool ContainsManufacturer(int id) =>
        catalogue.Items.Any(i => i.Manufacturer.Id == id);

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
        IManufacturersService? manufacturersService = ServiceLocator.GetService<IManufacturersService>();
        Manufacturer? manufacturer = await manufacturersService.GetAsync(item.Manufacturer.Name);
        if (manufacturer == null) throw new ArgumentNullException(nameof(manufacturer), "Производитель не найден.");
        TrimSpaces(item);
        item.Manufacturer = manufacturer;
        return await catalogue.CreateAsync(item);
    }

    public async Task EditAsync(Item item)
    {
        TrimSpaces(item);
        await catalogue.EditAsync(item);
    }

    public async Task RemoveAsync(Item item)
    {
        // Проверяем наличие материала на складе и 
        // использование материала в качестве израсходованного
        IStoreService? storeService = ServiceLocator.GetService<IStoreService>();
        bool existsInStore = storeService.ContainsItem(item);
        if (existsInStore) throw new ArgumentException(MessageContainer.ItemUsedInStore);
        IWastedItemsService? wastedItemsService = ServiceLocator.GetService<IWastedItemsService>();
        bool existsInWasteditems = wastedItemsService.GetItem(item.Manufacturer.Name, item.Article, item.Color) != null;
        if (existsInWasteditems) throw new ArgumentException(MessageContainer.ItemUsedInService);

        // Удалить материал из каталога
        await catalogue.RemoveAsync(item.Id);
    }

    private static void TrimSpaces(Item item)
    {
        item.Article = item.Article.Trim();
        item.Color = item.Color.Trim();
    }

}
