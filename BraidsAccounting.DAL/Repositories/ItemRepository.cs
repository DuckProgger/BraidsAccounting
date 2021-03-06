using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories;

/// <summary>
/// Репозиторий для материалов.
/// </summary>
internal class ItemRepository : DbRepository<Item>
{
    public override IQueryable<Item> Items => base.Items
        .Include(item => item.Manufacturer)
        ;

    public ItemRepository(ApplicationContext context) : base(context)
    {
    }
}
