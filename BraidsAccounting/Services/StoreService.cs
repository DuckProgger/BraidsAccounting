using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Реализация сервиса <see cref = "IStoreService" />.
    /// </summary>
    internal class StoreService : IStoreService
    {
        private readonly IRepository<StoreItem> store;
        private readonly IRepository<Item> items;
        private readonly IRepository<Manufacturer> manufacturers;

        public StoreService(
            IRepository<StoreItem> store,
            IRepository<Item> items,
            IRepository<Manufacturer> manufacturers)
        {
            this.store = store;
            this.items = items;
            this.manufacturers = manufacturers;
        }

        public void AddItem(StoreItem? storeItem)
        {
            if (storeItem == null) throw new ArgumentNullException(nameof(storeItem));
            if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));
            // Подгрузить из БД производителя с ценой, чтобы не создавать новую запись
            Manufacturer? manufacturer = GetManufacturer(storeItem.Item.Manufacturer.Name);
            // Если такого производителя нет, то выдать ошибку,
            // потому что производителей надо добавлять в отдельном окне
            storeItem.Item.Manufacturer = manufacturer is not null
                ? manufacturer
                : throw new Exception("Такого производителя нет в базе.");
            // Найти товар на складе
            Item? existingItem = GetItem(storeItem.Item);
            // Продукта в каталоге нет - надо добавить
            if (existingItem is null)
            {
                AddNewItem(storeItem);
                return;
            }
            StoreItem? existingStoreItem = store.Get(existingItem.Id);
            // Продукт в каталоге есть, но нет на складе - добавить на склад
            if (existingStoreItem is null)
            {
                AddExistingItem(storeItem, existingItem);
                return;
            }
            // Продукт есть в каталоге и на складе - изменить количество на складе
            existingStoreItem.Count += storeItem.Count;
            store.Edit(existingStoreItem);
        }


        /// <summary>
        /// Получить производителя по имени.
        /// </summary>
        /// <param name="name">Имя производителя.</param>
        /// <returns></returns>
        private Manufacturer? GetManufacturer(string name)=>
            manufacturers.Items.FirstOrDefault(
                m => m.Name.ToUpper() == name.ToUpper());

        /// <summary>
        /// Получить материал со склада.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Item? GetItem(Item item)=>
            items.Items
                .AsEnumerable()
                .FirstOrDefault(i => i.Equals(item));

        /// <summary>
        /// Добавить новый материал в каталог материалов и на склад.
        /// </summary>
        /// <param name="addedItem">Добавляемый материал.</param>
        private void AddNewItem(StoreItem addedItem)
        {
            Item? newItem = items.Create(addedItem.Item);
            addedItem.Item = newItem;
            store.Create(addedItem);
        }

        /// <summary>
        /// Добавить новый материал на склад.
        /// </summary>
        /// <param name="storeItem">Новый материал на складе.</param>
        /// <param name="item">Материал из каталога.</param>
        private void AddExistingItem(StoreItem storeItem, Item item)
        {
            storeItem.Item = item;
            store.Create(storeItem);
        }

        public IEnumerable<StoreItem?> GetItems() => store.Items;
        public void RemoveItems(IEnumerable<WastedItem?> wastedItems)
        {
            if (wastedItems == null) throw new ArgumentNullException(nameof(wastedItems));
            foreach (WastedItem? wastedItem in wastedItems)
            {
                if (wastedItem == null) throw new ArgumentNullException(nameof(wastedItem));
                StoreItem? existingStoreItem = store.Items.First(si => si.Item.Id == wastedItem.Item.Id);
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
            if (storeItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(storeItem));
            store.Edit(storeItem);
            items.Edit(storeItem.Item);
        }

        public void RemoveItem(int id) => store.Remove(id);

        public int GetItemCount(int id) => store.Items.First(si => si.Item.Id == id).Count;

    }
}
