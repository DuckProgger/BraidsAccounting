using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL.Context
{
    public class ApplicationContext : DbContext
    {
        DbSet<Item> Items { get; set; }
        DbSet<Service> Services { get; set; }
        DbSet<StoreItem> Store { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Ignore<EnumerableItems>();
        //    base.OnModelCreating(modelBuilder);
        //}

    }
}
