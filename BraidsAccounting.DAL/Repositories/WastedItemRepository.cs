﻿using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories
{
    internal class WastedItemRepository : DbRepository<WastedItem>
    {
        public override IQueryable<WastedItem> Items => base.Items
            .Include(wi => wi.Item)
            .ThenInclude(item => item.ItemPrice)
            ;

        public WastedItemRepository(ApplicationContext context) : base(context)
        {
        }
    }
}