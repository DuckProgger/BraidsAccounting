using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class WastedItemsService : IWastedItemsService
    {
        private readonly IRepository<WastedItem> wastedItems;
        private readonly IRepository<Service> services;

        public WastedItemsService(
            IRepository<WastedItem> wastedItems
            , IRepository<Service> services
            )
        {
            this.wastedItems = wastedItems;
            this.services = services;
        }

        public IEnumerable<WastedItemForm> GetWastedItemForms(string? workerName, bool grouping)
        {
            IQueryable<WastedItemForm> totalQuery;
            var baseQuery = GetBaseQuery();
            if (!string.IsNullOrEmpty(workerName)) AddFilter(ref baseQuery, workerName);
            if (!grouping) totalQuery = AddSelect(baseQuery);
            else totalQuery = AddSelectWithGrouping(baseQuery);
            return totalQuery;
        }

        private IQueryable<WastedItemQuery> GetBaseQuery()
        {
            return wastedItems.Items
               .Join(services.Items, w => w.ServiceId, s => s.Id, (w, s) => new WastedItemQuery
               {
                   WorkerName = s.Name,
                   ItemName = w.Item.Manufacturer.Name,
                   Article = w.Item.Article,
                   Color = w.Item.Color,
                   Count = w.Count,
                   Price = w.Item.Manufacturer.Price
               });
        }

        private static void AddFilter(ref IQueryable<WastedItemQuery> query, string workerName)
        {
            query = query.Where(w => w.WorkerName == workerName);
        }

        private static IQueryable<WastedItemForm> AddSelect(IQueryable<WastedItemQuery> query)
        {
            return query.Select(w => new WastedItemForm()
            {
                Article = w.Article,
                ItemName = w.ItemName,
                Color = w.Color,
                Count = w.Count,
                Expense = w.Count * w.Price
            });
        }

        private static IQueryable<WastedItemForm> AddSelectWithGrouping(IQueryable<WastedItemQuery> query)
        {
            return query.GroupBy(w => new
            {
                w.ItemName,
                w.Article,
                w.Color
            }
                ).Select(g =>
                new WastedItemForm
                {
                    ItemName = g.Key.ItemName,
                    Article = g.Key.Article,
                    Color = g.Key.Color,
                    Count = g.Sum(w => w.Count),
                    Expense = g.Sum(w => w.Count * w.Price)
                });
        }

        private class WastedItemQuery
        {
            public string? WorkerName { get; set; }
            public string? ItemName { get; set; }
            public string? Article { get; set; }
            public string? Color { get; set; }
            public int Count { get; set; }
            public decimal Price { get; set; }
        }
    }
}
