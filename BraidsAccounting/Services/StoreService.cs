﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

/// <summary>
/// Реализация сервиса <see cref = "IStoreService" />.
/// </summary>
internal class StoreService : IStoreService, IHistoryTracer<StoreItem>
{
    private readonly IRepository<StoreItem> store;
    private readonly ICatalogueService catalogue;
    private readonly IHistoryService historyService;

    public StoreService(
        IRepository<StoreItem> store
        , ICatalogueService catalogue
        , IHistoryService historyService)
    {
        this.store = store;
        this.catalogue = catalogue;
        this.historyService = historyService;
    }

    public async Task AddItemAsync(StoreItem? storeItem)
    {
        if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
        if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));
        // Подгрузить из БД производителя с ценой, чтобы не создавать новую запись
        var manufacturersService = ServiceLocator.GetService<IManufacturersService>();
        Manufacturer? manufacturer = await manufacturersService.GetAsync(storeItem.Item.Manufacturer.Name);
        // Если такого производителя нет, то выдать ошибку,
        // потому что производителей надо добавлять в отдельном окне
        storeItem.Item.Manufacturer = manufacturer is not null
            ? manufacturer
            : throw new Exception("Такого производителя нет в базе.");
        // Найти материал в каталоге
        Item? existingItem = await catalogue
            .GetAsync(storeItem.Item.Manufacturer.Name, storeItem.Item.Article, storeItem.Item.Color);
        // Такого материала нет в каталоге - добавить в каталог и на склад
        if (existingItem is null)
        {
            await AddNewItemAsync(storeItem);
            return;
        }
        // Найти материал на складе
        StoreItem? existingStoreItem = GetItem(existingItem.Manufacturer.Name, existingItem.Article, existingItem.Color);
        // Продукт в каталоге есть, но нет на складе - добавить на склад
        if (existingStoreItem is null)
        {
            await AddExistingItemAsync(storeItem, existingItem);
            return;
        }
        // Продукт есть в каталоге и на складе - изменить количество на складе
        existingStoreItem.Count += storeItem.Count;
        await EditItemAsync(existingStoreItem);
    }

    public StoreItem? GetItem(string manufacturer, string article, string color) =>
      store.Items.FirstOrDefault(i =>
      i.Item.Manufacturer.Name == manufacturer
      && i.Item.Article == article
      && i.Item.Color == color);

    /// <summary>
    /// Добавить новый материал в каталог материалов и на склад.
    /// </summary>
    /// <param name="addedItem">Добавляемый материал.</param>
    private async Task AddNewItemAsync(StoreItem addedItem)
    {
        Item newItem = await catalogue.AddAsync(addedItem.Item);
        addedItem.Item = newItem;
        await store.CreateAsync(addedItem);
        await historyService.WriteCreateOperationAsync(addedItem.GetEtityData(this));
    }

    /// <summary>
    /// Добавить новый материал на склад.
    /// </summary>
    /// <param name="storeItem">Новый материал на складе.</param>
    /// <param name="item">Материал из каталога.</param>
    private async Task AddExistingItemAsync(StoreItem storeItem, Item item)
    {
        storeItem.Item = item;
        await store.CreateAsync(storeItem);
        await historyService.WriteCreateOperationAsync(storeItem.GetEtityData(this));
    }

    public async Task<List<StoreItem>> GetItemsAsync() => await store.Items.ToListAsync();
    public async Task RemoveItemsAsync(IEnumerable<WastedItem?> wastedItems)
    {
        if (wastedItems == null) throw new ArgumentNullException(nameof(wastedItems));
        foreach (WastedItem? wastedItem in wastedItems)
        {
            if (wastedItem == null) throw new ArgumentNullException(nameof(wastedItem));
            StoreItem? existingStoreItem = await store.Items.FirstAsync(si => si.Item.Id == wastedItem.Item.Id);
            if (existingStoreItem is null) throw new Exception("Товар не найден в БД");
            var newStoreItem = existingStoreItem with { };
            newStoreItem.Count -= wastedItem.Count;
            switch (newStoreItem.Count)
            {
                case > 0:
                    await store.EditAsync(newStoreItem);
                    await historyService.WriteUpdateOperationAsync(existingStoreItem.GetEtityData(this), newStoreItem.GetEtityData(this));
                    break;
                case 0:
                    await store.RemoveAsync(newStoreItem.Id);
                    await historyService.WriteDeleteOperationAsync(newStoreItem.GetEtityData(this));
                    break;
                default:
                    throw new Exception("Указанного количества товара нет на складе");
            }
        }
    }

    public async Task EditItemAsync(StoreItem? storeItem)
    {
        if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
        if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));
        var existingStoreItem = store.Get(storeItem.Id);
        await store.EditAsync(storeItem);
        await catalogue.EditAsync(storeItem.Item);
        await historyService.WriteUpdateOperationAsync(existingStoreItem.GetEtityData(this), storeItem.GetEtityData(this));

    }

    public async Task RemoveItemAsync(int id)
    {
        var existingStoreItem = await store.GetAsync(id);
        await store.RemoveAsync(id);
        await historyService.WriteDeleteOperationAsync(existingStoreItem.GetEtityData(this));
    }

    public int GetItemCount(string manufacturer, string article, string color)
    {
        var storeItem = GetItem(manufacturer, article, color);
        if (storeItem is null) return 0;
        return storeItem.Count;
    }

    public bool ContainsItem(Item item) =>
        store.Items.Any(si => si.Item.Equals(item));

    public IEntityDataBuilder<StoreItem> ConfigureEntityData(IEntityDataBuilder<StoreItem> builder, StoreItem entity) =>
        builder
        .AddInfo(s => s.Item.Manufacturer.Name, entity.Item.Manufacturer.Name)
        .AddInfo(s => s.Item.Article, entity.Item.Article)
        .AddInfo(s => s.Item.Color, entity.Item.Color)
        .AddInfo(s => s.Count, entity.Count);
}
