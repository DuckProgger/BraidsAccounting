using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class StoreService : IStoreService
    {
        private readonly IRepository<StoreItem> store;
        private readonly IRepository<Item> items;
        private readonly IRepository<ItemPrice> itemPrices;

        public StoreService(
            IRepository<StoreItem> store,
            IRepository<Item> items,
            IRepository<ItemPrice> itemPrices)
        {
            this.store = store;
            this.items = items;
            this.itemPrices = itemPrices;
        }

        public void AddItem(StoreItem? storeItem)
        {
            if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
            //if (!itemPrices.Items.AsEnumerable().Any(ip => ip.Equals(storeItem.Item.ItemPrice)))
            //    throw new Exception("Такого производителя нет в базе.");
            // Подгрузить из БД производителя с ценой, чтобы не создавать новую запись
            var itemPrice = itemPrices.Items.FirstOrDefault(
                ip => ip.Manufacturer/*.ToUpper()*/ == storeItem.Item.ItemPrice.Manufacturer/*.ToUpper()*/);
            // Если такого производителя нет, то выдать ошибку,
            // потому что производителей надо добавлять в отдельном окне
            storeItem.Item.ItemPrice = itemPrice is not null 
                ? itemPrice 
                : throw new Exception("Такого производителя нет в базе."); 
            // Найти товар на складе
            var existingItem = items.Items
                .AsEnumerable()
                .FirstOrDefault(i => i.Equals(storeItem.Item));
            // Продукта в каталоге нет - надо добавить
            if (existingItem is null)
            {
                var item = items.Create(storeItem.Item);
                storeItem.Item = item;
                store.Create(storeItem);
                return;
            }
            var existingStoreItem = store.Items.FirstOrDefault(si => si.Item.Id == existingItem.Id);
            // Продукт в каталоге есть, но нет на складе - добавить на склад
            if (existingStoreItem is null)
            {
                storeItem.Item = existingItem;
                store.Create(storeItem);
                return;
            }
            // Продукт есть в каталоге и на складе - изменить количество на складе
            existingStoreItem.Count += storeItem.Count;
            store.Edit(existingStoreItem);
        }

        public IEnumerable<StoreItem?> GetItems() => store.Items;
        public void RemoveItems(IEnumerable<WastedItem?> wastedItems)
        {
            if (wastedItems == null) throw new ArgumentNullException(nameof(wastedItems));
            foreach (var wastedItem in wastedItems)            {
                if (wastedItem == null) throw new ArgumentNullException(nameof(wastedItem));
                var existingStoreItem = store.Items.First(si => si.ItemId == wastedItem.Item.Id);
                if (existingStoreItem is null) throw new Exception("Товар не найден в БД");
                existingStoreItem.Count -= wastedItem.Count;
                switch (existingStoreItem.Count)
                {
                    case > 0:
                        store.Edit(existingStoreItem);
                        break;
                    case 0:
                        store.Remove(existingStoreItem.Id);
                        break;
                    default:
                        throw new Exception("Указанного количества товара нет на складе");
                }
            }
        }

        public void EditItem(StoreItem? storeItem)
        {
            if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
            store.Edit(storeItem);
        }

        public void RemoveItem(int id)
        {
            store.Remove(id);
        }
    }
}
