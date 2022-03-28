using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Mvvm;
using System;

namespace BraidsAccounting.Models
{
    /// <summary>
    /// Представляет класс для отображения информации о материале в форме.
    /// </summary>
    internal class FormItem : BindableBase, IEquatable<StoreItem>
    {
        private int? maxCount;
        private int count;

        /// <summary>
        /// ID материала.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название производителя.
        /// </summary>
        public string Manufacturer { get; set; } = null!;
        /// <summary>
        /// Стоимость материала.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Артикул материала.
        /// </summary>
        public string Article { get; set; } = null!;
        /// <summary>
        /// Цвет материала.
        /// </summary>
        public string Color { get; set; } = null!;
        /// <summary>
        /// Количество материалов.
        /// </summary>
        public int Count { get => count; set => count = Math.Min(value, MaxCount); }
        /// <summary>
        /// Максимально допустимое количество материалов, исходя из наличия на складе.
        /// </summary>
        public int MaxCount
        {
            get
            {
                if (maxCount is null)
                {
                    // Получить сервис работы со складом
                    IStoreService? store = ServiceLocator.GetService<IStoreService>();
                    maxCount = store.GetItemCount(Id);
                }
                return maxCount.Value;
            }
        }

        public static implicit operator FormItem(StoreItem storeItem)
        {
            return new()
            {
                Count = storeItem.Count,
                Article = storeItem.Item.Article,
                Manufacturer = storeItem.Item.Manufacturer.Name,
                Price = storeItem.Item.Manufacturer.Price,
                Color = storeItem.Item.Color,
                Id = storeItem.Item.Id
            };
        }
        public static implicit operator WastedItem(FormItem formItem)
        {
            Manufacturer itemPrice = new()
            {
                Name = formItem.Manufacturer,
                Price = formItem.Price
            };
            Item item = new()
            {
                Id = formItem.Id,
                Article = formItem.Article,
                Color = formItem.Color,
                Manufacturer = itemPrice
            };
            return new WastedItem()
            {
                Count = formItem.Count,
                Item = item
            };
        }

        public bool Equals(StoreItem? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return
                Manufacturer == other.Item.Manufacturer.Name
                && Price == other.Item.Manufacturer.Price
                && Color == other.Item.Color
                && Article == other.Item.Article;
        }
    }
}
