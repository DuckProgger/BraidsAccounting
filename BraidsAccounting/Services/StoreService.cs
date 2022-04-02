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
    /// Реализация сервиса <see cref = "IStoreService" />.
    /// </summary>
    internal class StoreService : IStoreService
    {
        private readonly IRepository<StoreItem> store;
        private readonly IItemsService catalogue;
        private readonly IRepository<Manufacturer> manufacturers;

        public StoreService(
            IRepository<StoreItem> store,
            IItemsService catalogue,
            IRepository<Manufacturer> manufacturers)
        {
            this.store = store;
            this.catalogue = catalogue;
            this.manufacturers = manufacturers;
        }

        public async Task AddItemAsync(StoreItem? storeItem)
        {
            if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
            if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));
            // Подгрузить из БД производителя с ценой, чтобы не создавать новую запись
            Manufacturer? manufacturer = await GetManufacturerAsync(storeItem.Item.Manufacturer.Name);
            // Если такого производителя нет, то выдать ошибку,
            // потому что производителей надо добавлять в отдельном окне
            storeItem.Item.Manufacturer = manufacturer is not null
                ? manufacturer
                : throw new Exception("Такого производителя нет в базе.");
            // Найти материал в каталоге
            Item? existingItem = await catalogue
                .GetItemAsync(storeItem.Item.Manufacturer.Name, storeItem.Item.Article, storeItem.Item.Color);
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
        /// Получить производителя по имени.
        /// </summary>
        /// <param name="name">Имя производителя.</param>
        /// <returns></returns>
        private async Task<Manufacturer?> GetManufacturerAsync(string name) =>
           await manufacturers.Items.FirstOrDefaultAsync(
                m => m.Name.ToUpper() == name.ToUpper());

        /// <summary>
        /// Добавить новый материал в каталог материалов и на склад.
        /// </summary>
        /// <param name="addedItem">Добавляемый материал.</param>
        private async Task AddNewItemAsync(StoreItem addedItem)
        {
            Item newItem = await catalogue.AddAsync(addedItem.Item);
            addedItem.Item = newItem;
            await store.CreateAsync(addedItem);
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
                existingStoreItem.Count -= wastedItem.Count;
                switch (existingStoreItem.Count)
                {
                    case > 0:
                        await store.EditAsync(existingStoreItem);
                        break;
                    case 0:
                        await store.RemoveAsync(existingStoreItem.Id);
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
            await store.EditAsync(storeItem);
            await catalogue.EditAsync(storeItem.Item);
        }

        public async Task RemoveItemAsync(int id) => await store.RemoveAsync(id);

        public int GetItemCount(string manufacturer, string article, string color)
        {
            var storeItem = GetItem(manufacturer, article, color);
            if (storeItem is null) return 0;
            return storeItem.Count;
        }
    }
}
