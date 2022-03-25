using BraidsAccounting.DAL.Entities;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Services;

namespace BraidsAccounting.Models
{
    internal class FormItem : BindableBase, IEquatable<StoreItem>
    {
        private int? maxCount;
        private int count;

        public int Id { get; set; }
        public string Manufacturer { get; set; } = null!;
        public decimal Price { get; set; }
        public string Article { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Count { get => count; set => count = Math.Min(value, MaxCount); }
        public int MaxCount
        {
            get
            {
                if (maxCount is null)
                {
                    var store = ServiceLocator.GetService<IStoreService>();
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
