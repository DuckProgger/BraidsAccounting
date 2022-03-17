using BraidsAccounting.DAL.Context;
using BraidsAccounting.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL
{
    internal class DbRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly ApplicationContext context;
        private readonly DbSet<T> dbSet;
        public DbRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual IQueryable<T> Items => dbSet;

        public T? Get(int id) => Items.SingleOrDefault(item => item.Id == id);

        public async Task<T?> GetAsync(int id, CancellationToken cancel = default) => await Items.
            SingleOrDefaultAsync(item => item.Id == id, cancel)
            .ConfigureAwait(false);

        public T Create(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            context.Entry(item).State = EntityState.Added;
            context.SaveChanges();
            return item;
        }

        public async Task<T?> CreateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            context.Entry(item).State = EntityState.Added;
            await context.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }

        public void Edit(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public async Task EditAsync(T item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public void Delete(int id)
        {
            T item = new() { Id = id };
            dbSet.Remove(item);
            context.SaveChanges();
        }

        public async Task DeleteAsync(int id, CancellationToken cancel = default)
        {
            T item = new() { Id = id };
            dbSet.Remove(item);
            await context.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
