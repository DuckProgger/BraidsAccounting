using BraidsAccounting.DAL.Context;
using BraidsAccounting.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL.Repositories
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
            context.Add(item);
            context.SaveChanges();
            return item;
        }

        public async Task<T?> CreateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            //context.Entry(item).State = EntityState.Added;
            await context.AddAsync(item, cancel);
            await context.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }

        public void CreateRange(IEnumerable<T> items)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            context.AddRange(items);
            context.SaveChanges();
        }

        public async Task CreateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            await context.AddRangeAsync(items, cancel);
            await context.SaveChangesAsync(cancel);
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

        public void EditRange(IEnumerable<T> items)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
                context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task EditRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
                context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync(cancel);
        }
        public void Remove(int id)
        {
            T item = new() { Id = id };
            dbSet.Remove(item);
            context.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            T item = new() { Id = id };
            dbSet.Remove(item);
            await context.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
            context.SaveChanges();
        }
        public async Task RemoveRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            dbSet.RemoveRange(items);
            await context.SaveChangesAsync(cancel);
        }
    }
}
