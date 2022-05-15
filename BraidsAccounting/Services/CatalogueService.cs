﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Exceptions;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
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
internal class CatalogueService : ICatalogueService, IHistoryTracer<Item>
{
    private readonly IRepository<Item> catalogue;
    private readonly IHistoryService historyService;

    public CatalogueService(IRepository<Item> catalogue, IHistoryService historyService)
    {
        this.catalogue = catalogue;
        this.historyService = historyService;
    }

    public Item? Get(string manufacturer, string article, string color) =>
      catalogue.Items.FirstOrDefault(i =>
      i.Manufacturer.Name == manufacturer
      && i.Article == article
      && i.Color == color);
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
            .Where(i => i.StoreItems.Any(si => si.Item.Id == i.Id))
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
        // Контроль дубликата
        if (await Contains(item)) throw new DublicateException("Материал уже есть в каталоге.");
        var newItem = await catalogue.CreateAsync(item);
        await historyService.WriteCreateOperationAsync(item.GetEtityData(this));
        return newItem;
    }

    public async Task<bool> Contains(Item item)
    {
        var array = await catalogue.Items.ToArrayAsync();
        return array.Any(i => i.Equals(item));
    }

    public async Task EditAsync(Item item)
    {
        TrimSpaces(item);       
        var existingItem = await catalogue.GetAsync(item.Id) with { }; // Получить актуальную сущность
        await catalogue.EditAsync(item);
        await historyService.WriteUpdateOperationAsync(existingItem.GetEtityData(this), item.GetEtityData(this));
    }

    public async Task RemoveAsync(Item item)
    {
        // Проверяем наличие материала на складе и 
        // использование материала в качестве израсходованного
        IStoreService? storeService = ServiceLocator.GetService<IStoreService>();
        if (await storeService.ContainsItemAsync(item.Id)) 
            throw new ArgumentException(Messages.ItemUsedInStore);
        IWastedItemsService? wastedItemsService = ServiceLocator.GetService<IWastedItemsService>();
        if (await wastedItemsService.ContainsItemAsync(item.Id)) 
            throw new ArgumentException(Messages.ItemUsedInService);
        // Удалить материал из каталога
        await catalogue.RemoveAsync(item.Id);
        await historyService.WriteDeleteOperationAsync(item.GetEtityData(this));
    }

    private static void TrimSpaces(Item item)
    {
        item.Article = item.Article.Trim();
        item.Color = item.Color.Trim();
    }

    IEntityDataBuilder<Item> IHistoryTracer<Item>.ConfigureEntityData(IEntityDataBuilder<Item> builder, Item entity) =>
        builder
        .AddInfo(i => i.Manufacturer.Name, entity.Manufacturer.Name)
        .AddInfo(i => i.Article, entity.Article)
        .AddInfo(i => i.Color, entity.Color);
}
