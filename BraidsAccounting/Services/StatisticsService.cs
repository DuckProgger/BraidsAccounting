using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BraidsAccounting.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly IRepository<WastedItem> wastedItems;

        public StatisticsService(IRepository<WastedItem> wastedItems)
        {
            this.wastedItems = wastedItems;
        }

        public IEnumerable<WastedItemForm> GetWastedItemForms(StatisticsFilterOptions options)
        {
            IQueryable<WastedItemForm> totalQuery;
            IQueryable<WastedItem>? baseQuery = wastedItems.Items;
            if (options.EnableWorkerFilter) AddWorkerFilter(ref baseQuery, options.WorkerNameFilter);
            if (options.EnablePeriodFilter) AddPeriodFilter(ref baseQuery, options.DatePeriod);
            if (options.EnableGrouping) totalQuery = AddSelectWithGrouping(baseQuery);
            else totalQuery = AddSelect(baseQuery);
            return totalQuery;
        }

        private static void AddWorkerFilter(ref IQueryable<WastedItem> query, string? workerName)
        {
            if (workerName is null) throw new ArgumentNullException(nameof(workerName));
            query = query.Where(w => w.Service.Name == workerName);
        }

        private static void AddPeriodFilter(ref IQueryable<WastedItem> query, DatePeriod period)
        {
            query = query.Where(w =>
            w.Service.DateTime.Date >= period.Start
            && w.Service.DateTime.Date <= period.End
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
                ItemName = w.Item.Manufacturer.Name,
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
