using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using System.Linq;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Реализация сервиса <see cref = "IItemsService" />.
    /// </summary>
    internal class ItemsService : IItemsService
    {
        private readonly IRepository<Item> items;

        public ItemsService(IRepository<Item> items)
        {
            this.items = items;
        }

        public bool ContainsManufacturer(string manufacturerName) => 
            items.Items.Any(i => i.Manufacturer.Name == manufacturerName);
    }
}
