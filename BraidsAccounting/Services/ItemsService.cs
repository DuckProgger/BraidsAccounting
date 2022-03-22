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

        public ItemsService(IRepository<Item> items)
        {
            this.items = items;
        }

        public IEnumerable<string> GetManufacturers() => items.Items.Select(i => i.ItemPrice.Manufacturer).Distinct();

    }
}
