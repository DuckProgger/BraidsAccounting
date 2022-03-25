using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories
{
    internal class ServiceRepository : DbRepository<Service>
    {
        public override IQueryable<Service> Items => base.Items
            .Include(s => s.WastedItems)
            .ThenInclude(ei => ei.Item)
            .ThenInclude(i => i.Manufacturer)
            ;
        public ServiceRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
