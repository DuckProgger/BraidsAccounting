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

        public IEnumerable<WastedItemForm> GetWastedItemForms(string? workerName, bool grouping, DatePeriod period)
        {
            IQueryable<WastedItemForm> totalQuery;
            var baseQuery = wastedItems.Items;
            if (!string.IsNullOrEmpty(workerName)) AddFilter(ref baseQuery, workerName, period);
            if (!grouping) totalQuery = AddSelect(baseQuery);
            else totalQuery = AddSelectWithGrouping(baseQuery);
            return totalQuery;
        }

        private static void AddFilter(ref IQueryable<WastedItem> query, string workerName, DatePeriod period)
        {
            query = query.Where(w =>
            w.Service.Name == workerName 
            && w.Service.DateTime.Date > period.Start
            && w.Service.DateTime.Date < period.End
            );
        }

        private static IQueryable<WastedItemForm> AddSelect(IQueryable<WastedItem> query)
        {
            return query.Select(w => new WastedItemForm()
            {
                Article = w.Item.Article,
                ItemName = w.Item.Manufacturer.Name,
                Color = w.Item.Color,
                Count = w.Count,
                Expense = w.Count * w.Item.Manufacturer.Price
            });
        }

        private static IQueryable<WastedItemForm> AddSelectWithGrouping(IQueryable<WastedItem> query)
        {
            return query.GroupBy(w => new
            {
                ItemName = w.Service.Name,
                w.Item.Article,
                w.Item.Color
            }
                ).Select(g =>
                new WastedItemForm
                {
                    ItemName = g.Key.ItemName,
                    Article = g.Key.Article,
                    Color = g.Key.Color,
                    Count = g.Sum(w => w.Count),
                    Expense = g.Sum(w => w.Count * w.Item.Manufacturer.Price)
                });
        }
    }
}
