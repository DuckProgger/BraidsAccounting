using BraidsAccounting.DAL.Entities;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Models
{
    internal class FormItem : BindableBase
    {
        public FormItem(string manufacturer, decimal price, string article, string color, int count)
        {
            Manufacturer = manufacturer;
            Price = price;
            Article = article;
            Color = color;
            Count = count;
        }

        public string Manufacturer { get; set; } = null!;
        public decimal Price { get; set; }
        public string Article { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Count { get; set; } 
        
        //public WastedItem ToWastedItem()
        //{
        //    return new() 
        //    { 
                
        //    };

        //}
    }
}
