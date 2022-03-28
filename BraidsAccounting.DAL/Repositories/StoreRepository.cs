using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для склада.
    /// </summary>
    internal class StoreRepository : DbRepository<StoreItem>
    {
        public override IQueryable<StoreItem> Items => base.Items
            .Include(si => si.Item)
            .ThenInclude(item => item.Manufacturer)
            ;

        public StoreRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
