using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Services;
using Microsoft.EntityFrameworkCore;

class Program
{
   
    static void Main()
    {

        const string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Braids;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        using var db = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(connection).Options);

        //ServiceRepository serviceRepository = new(db);
        //StoreRepository storeRepository = new(db);
        //ItemRepository itemRepository = new(db);
        //DbRepository<Manufacturer> manufacturerRepository = new(db);
        //WastedItemRepository wastedItemRepository = new(db);

        //StoreService storeService = new(storeRepository, itemRepository, manufacturerRepository);
        //ServiceProvider serviceProvider = new(serviceRepository, storeService, wastedItemRepository);

        // Входные данные
        string name = "Наташа";
        DateTime begin = new(2022, 01, 01);
        DateTime end = new(2022, 04, 01);


        //var query = from w in db.WastedItems.Include(w => w.Item).ThenInclude(i => i.Manufacturer)
        //            join s in db.Services
        //            on w.ServiceId equals s.Id
        //            select new
        //            { w.Item.Manufacturer.Name, w.Item.Article, w.Item.Color, w.Count, Expense = w.Count * w.Item.Manufacturer.Price };

        //var query = from w in db.WastedItems.Include(w => w.Item).ThenInclude(i => i.Manufacturer)
        //            join s in db.Services
        //            on w.ServiceId equals s.Id
        //            where s.Name == name
        //            select new
        //            { w.Item.Manufacturer.Name, w.Item.Article, w.Item.Color, w.Count, Expense = w.Count * w.Item.Manufacturer.Price };

        //var query = from w in db.WastedItems.Include(w => w.Item).ThenInclude(i => i.Manufacturer)
        //            join s in db.Services
        //            on w.ServiceId equals s.Id
        //            where s.Name == name
        //            group w by new
        //            {
        //                w.Item.Manufacturer.Name,
        //                w.Item.Article,
        //                w.Item.Color
        //            }
        //            into g
        //            select new
        //            {
        //                g.Key.Name,
        //                g.Key.Article,
        //                g.Key.Color,
        //                Count = g.Sum(w => w.Count),
        //                Expense = g.Sum(w => w.Count * w.Item.Manufacturer.Price)
        //            };


        //IQueryable<WastedItemQuery>? query = db.WastedItems.Include(w => w.Item).ThenInclude(i => i.Manufacturer)
        //    .Join(db.Services, w => w.ServiceId, s => s.Id, (w, s) => new WastedItemQuery
        //    {
        //        WorkerName = s.Name,
        //        ItemName = w.Item.Manufacturer.Name,
        //        Article = w.Item.Article,
        //        Color = w.Item.Color,
        //        Count = w.Count,
        //        Price = w.Item.Manufacturer.Price
        //    });

        //query = query.Where(w => w.WorkerName == name);

        //IQueryable<WastedItemForm>? query2 = query.Select(w => new WastedItemForm()
        //{
        //    Article = w.Article,
        //    ItemName = w.ItemName,
        //    Color = w.Color,
        //    Count = w.Count,
        //    Expense = w.Count * w.Price
        //});

        //var query3 = query.
        //    GroupBy(w => new
        //    {
        //        w.ItemName,
        //        w.Article,
        //        w.Color
        //    }
        //    ).Select(g =>
        //    new WastedItemForm
        //    {
        //        ItemName = g.Key.ItemName,
        //        Article = g.Key.Article,
        //        Color = g.Key.Color,
        //        Count = g.Sum(w => w.Count),
        //        Expense = g.Sum(w => w.Count * w.Price)
        //    });



        var query = GetWastedItemForm(db, "Наташа", true);

        //var queryStr = query.ToQueryString();

        var res = query.ToList();

    }

   static IEnumerable<WastedItemForm> GetWastedItemForm(ApplicationContext db, string? workerName, bool grouping)
    {
        IQueryable<WastedItemForm> totalQuery;
        var baseQuery = GetBaseQuery(db);
        if (!string.IsNullOrEmpty(workerName)) AddFilter(ref baseQuery, workerName);
        if (!grouping) totalQuery = AddSelect(baseQuery);
        else totalQuery = AddSelectWithGrouping(baseQuery);
        return totalQuery;
    }

    private static IQueryable<WastedItemQuery> GetBaseQuery(ApplicationContext db)
    {
        return db.WastedItems.Include(w => w.Item).ThenInclude(i => i.Manufacturer)
           .Join(db.Services, w => w.ServiceId, s => s.Id, (w, s) => new WastedItemQuery
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




    class WastedItemQuery
    {
        public string WorkerName { get; set; }
        public string ItemName { get; set; }
        public string Article { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }

    class WastedItemForm
    {
        public string ItemName { get; set; }
        public string Article { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public decimal Expense { get; set; }
    }
}