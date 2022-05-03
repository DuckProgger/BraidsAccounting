using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories;

/// <summary>
/// Репозиторий для израсходованных материалов.
/// </summary>
internal class WastedItemRepository : DbRepository<WastedItem>
{
    public override IQueryable<WastedItem> Items => base.Items
        .Include(wi => wi.Item)
        .ThenInclude(item => item.Manufacturer)
        .Include(wi => wi.Service)
        ;

    public WastedItemRepository(ApplicationContext context) : base(context)
    {
    }
}
