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

        public void AddItem(StoreItem storeItem)
        {
            if (!itemPrices.Items.AsEnumerable().Any(ip => ip.Equals(storeItem.Item.ItemPrice)))
                throw new Exception("Такого производителя нет в базе.");
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

        public IEnumerable<StoreItem> GetItems() => throw new NotImplementedException();
        public void RemoveItems(IEnumerable<WastedItem> wastedItems)
        {
            //var storeItems = store.Items.Select(si => si).Where(si => wastedItems.Any(wi => wi.Item.Id == si.Item.Id));
            //foreach (var storeItem in storeItems)
            //    storeItem.Count -=

            List<StoreItem> storeItems = new();
            foreach (var wastedItem in wastedItems)
            {
                var existingStoreItem = store.Get(wastedItem.ItemId);
                if (existingStoreItem is null) throw new Exception("Товар не найден в БД");
                existingStoreItem.Count -= wastedItem.Count;
                storeItems.Add(existingStoreItem);
            }
            store.EditRange(storeItems);
        }

        //private void AddStoreItem(StoreItems storeItem)
        //{
        //    var existingItem = itemsRep.Items
        //        .AsEnumerable()
        //        .FirstOrDefault(i => i.Equals(storeItem.EnumerableItem.Item));
        //    // Продукта в каталоге нет - надо добавить
        //    if (existingItem is null)
        //    {
        //        var item = itemsRep.Create(storeItem.EnumerableItem.Item);
        //        storeItem.EnumerableItem.Item = item;
        //        storeRep.Create(storeItem);
        //        return;
        //    }
        //    var existingStoreItem = storeRep.Items
        //        .AsEnumerable()
        //        .FirstOrDefault(si => si.EnumerableItem.Equals(storeItem.EnumerableItem));
        //    // Продукт в каталоге есть, но нет на складе - добавить на склад
        //    if (existingStoreItem is null)
        //    {
        //        storeItem.EnumerableItem.Item = existingItem;
        //        storeRep.Create(storeItem);
        //        return;
        //    }
        //    // Продукт есть в каталоге и на складе - изменить количество на складе
        //    existingStoreItem.EnumerableItem.Count += storeItem.EnumerableItem.Count;
        //    storeRep.Edit(existingStoreItem);
        //}
    }
}
