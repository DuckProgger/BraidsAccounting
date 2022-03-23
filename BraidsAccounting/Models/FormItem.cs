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
    internal class FormItem : BindableBase
    {
        private int? maxCount;
        private int count;

        //public FormItem(/*string manufacturer, decimal price, string article, string color, int count*/)
        //{
        //    //Manufacturer = manufacturer;
        //    //Price = price;
        //    //Article = article;
        //    //Color = color;
        //    //Count = count;

        //    //var store = ServiceLocator.GetService<IStoreService>();
        //    //MaxCount = store.
        //}

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
                Manufacturer = storeItem.Item.ItemPrice.Manufacturer,
                Price = storeItem.Item.ItemPrice.Price,
                Color = storeItem.Item.Color,
                Id = storeItem.Item.Id
            };
        }
        public static implicit operator WastedItem(FormItem formItem)
        {
            ItemPrice itemPrice = new()
            {
                Manufacturer = formItem.Manufacturer,
                Price = formItem.Price
            };
            Item item = new()
            {
                Id = formItem.Id,
                Article = formItem.Article,
                Color = formItem.Color,
                ItemPrice = itemPrice
            };
            return new WastedItem()
            {
                Count = formItem.Count,
                Item = item
            };
        }
    }
}
