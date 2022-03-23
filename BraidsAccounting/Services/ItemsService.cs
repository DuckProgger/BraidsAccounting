using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class ItemsService : IItemsService
    {
        private readonly IRepository<Item> items;
        //private readonly IRepository<Manufacturer> manufacturers;

        public ItemsService(IRepository<Item> items/*/*, IRepository<Manufacturer> manufacturers*/*/)
        {
            this.items = items;
            //this.manufacturers = manufacturers;
        }

        public bool ContainsManufacturer(string manufacturerName)
        {
           return items.Items.Any(i => i.Manufacturer.Name == manufacturerName);
        }

        //public IEnumerable<string> GetManufacturers() => manufacturers.Items.Select(m => m.Name);

            //items.Items.Select(i => i.Manufacturer.Name).Distinct();
    }
}
