using BraidsAccounting.DAL.Entities;
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
/// Реализация сервиса <see cref = "IStoreService" />.
/// </summary>
internal class StoreService : IStoreService
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

    public async Task AddAsync(StoreItem? storeItem)
    {
        if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
        if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));

        // Найти материал в каталоге
        var existingItem = await catalogue.GetAsync(storeItem.Item.Manufacturer.Name, storeItem.Item.Article, storeItem.Item.Color);
        if (existingItem is null)
            existingItem = await catalogue.AddAsync(storeItem.Item);
        storeItem.Item = existingItem;

        // Найти материал на складе
        var existingStoreItem = Get(storeItem.Item.Manufacturer.Name, storeItem.Item.Article, storeItem.Item.Color);

        // Продукта нет в каталоге - добавить в каталог и на склад
        if (existingStoreItem is null)
        {
            await AddInternalAsync(storeItem);
            return;
        }

        // Продукт есть каталоге - изменить количество
        StoreItem? existingStoreItemClone = existingStoreItem with { }; // Чтобы контекст БД видел различия
        existingStoreItemClone.Count += storeItem.Count;
        await EditAsync(existingStoreItemClone);
    }

    public StoreItem? Get(string manufacturer, string article, string color) =>
      store.Items.FirstOrDefault(i =>
      i.Item.Manufacturer.Name == manufacturer
      && i.Item.Article == article
      && i.Item.Color == color);

    public async Task<StoreItem?> GetByItemAsync(int itemId) =>
        await store.Items.FirstAsync(si => si.Item.Id == itemId);

    public async Task<StoreItem?> GetAsync(int id) =>
        await store.GetAsync(id);

    /// <summary>
    /// Добавить новый материал на склад.
    /// </summary>
    /// <param name="storeItem">Новый материал на складе.</param>
    /// <param name="item">Материал из каталога.</param>
    private async Task AddInternalAsync(StoreItem storeItem)
    {
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
            StoreItem? existingStoreItem = await GetByItemAsync(wastedItem.Item.Id);
            if (existingStoreItem is null) throw new Exception(Resources.StoreItemNotFound);
            var oldStoreItem = existingStoreItem with { };
            var newStoreItem = existingStoreItem with { };
            newStoreItem.Count -= wastedItem.Count;
            if(newStoreItem.Count < 0)
                throw new Exception(Resources.StoreItemOutOfStock);
            await store.EditAsync(newStoreItem);
            await historyService.WriteUpdateOperationAsync(oldStoreItem.GetEtityData(this), newStoreItem.GetEtityData(this));
        }
    }

    public async Task EditAsync(StoreItem? storeItem)
    {
        if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
        if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));
        var previousStoreItem = store.Get(storeItem.Id) with { };
        await store.EditAsync(storeItem);
        await catalogue.EditAsync(storeItem.Item);
        await historyService.WriteUpdateOperationAsync(previousStoreItem.GetEtityData(this), storeItem.GetEtityData(this));
    }

    public async Task RemoveAsync(int id)
    {
        var existingStoreItem = await store.GetAsync(id);
        await store.RemoveAsync(id);
        await historyService.WriteDeleteOperationAsync(existingStoreItem.GetEtityData(this));
    }

    public int Count(string manufacturer, string article, string color)
    {
        var storeItem = Get(manufacturer, article, color);
        if (storeItem is null) return 0;
        return storeItem.Count;
    }

    public async Task<bool> ContainsItemAsync(int itemId) =>
         await store.Items.AnyAsync(si => si.Item.Id == itemId);

    IEntityDataBuilder<StoreItem> IHistoryTracer<StoreItem>.ConfigureEntityData(IEntityDataBuilder<StoreItem> builder, StoreItem entity) =>
        builder
        .AddInfo(s => s.Item.Manufacturer.Name, entity.Item.Manufacturer.Name)
        .AddInfo(s => s.Item.Article, entity.Item.Article)
        .AddInfo(s => s.Item.Color, entity.Item.Color)
        .AddInfo(s => s.Count, entity.Count);

    public bool Validate(StoreItem entity, out IEnumerable<string> errorMessages)
    {
        List<string> errorMessagesList = new();
        errorMessages = errorMessagesList;
        bool haveError = false;
        if (entity.Count <= 0)
        {
            errorMessagesList.Add(Resources.InvalidStoreItemCount);
            haveError = true;
        }
        if (!catalogue.Validate(entity.Item, out IEnumerable<string> itemErrorMessages))
        {
            foreach (var itemErrorMessage in itemErrorMessages)
                errorMessagesList.Add(itemErrorMessage);
            haveError = true;
        }
        return !haveError;
    }


}
