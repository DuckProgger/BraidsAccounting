using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories
{
    internal class ServiceRepository : DbRepository<Service>
    {
        public override IQueryable<Service> Items => base.Items.Include(item => item.Items).ThenInclude(si => si.Item);
        public ServiceRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
