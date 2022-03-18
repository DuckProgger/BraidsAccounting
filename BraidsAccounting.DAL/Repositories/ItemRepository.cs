using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories
{
    internal class ItemRepository : DbRepository<Item>
    {
        public override IQueryable<Item> Items => base.Items
            .Include(item => item.ItemPrice)
            ;

        public ItemRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
