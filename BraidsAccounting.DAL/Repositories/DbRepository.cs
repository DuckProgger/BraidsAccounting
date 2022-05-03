using BraidsAccounting.DAL.Context;
using BraidsAccounting.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BraidsAccounting.DAL.Repositories;

/// <summary>
/// Базовый класс репозитория для сущности.
/// </summary>
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

    private bool CreateInternal(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        if (Items.Contains(item)) return true;
        context.Entry(item).State = EntityState.Added;
        return false;
    }

    public T Create(T item)
    {          
        if (CreateInternal(item)) return item;
        SaveChanges();
        return item;
    }

    public async Task<T> CreateAsync(T item, CancellationToken cancel = default)
    {
        if (CreateInternal(item)) return item;
        await SaveChangesAsync(cancel);
        return item;
    }

    public void CreateRange(IEnumerable<T> items)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        foreach (T? item in items)
            context.Entry(item).State = EntityState.Added;
        SaveChanges();
    }

    public async Task CreateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        await context.AddRangeAsync(items, cancel);
        await SaveChangesAsync(cancel);
    }

    private void EditInternal(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        T existingItem = Get(item.Id) ?? throw new Exception("Элемент не найден.");
        context.Entry(existingItem).CurrentValues.SetValues(item);
    }

    public void Edit(T item)
    {
        EditInternal(item);
        SaveChanges();
    }

    public async Task EditAsync(T item, CancellationToken cancel = default)
    {
        EditInternal(item);
        await SaveChangesAsync(cancel);
    }

    public void EditRange(IEnumerable<T> items)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        foreach (T? item in items)
            context.Entry(item).State = EntityState.Modified;
        SaveChanges();
    }
    public async Task EditRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        foreach (T? item in items)
            context.Entry(item).State = EntityState.Modified;
        await SaveChangesAsync(cancel);
    }
    public void Remove(int id)
    {
        T item = Get(id) ?? throw new Exception("Элемент не найден.");
        dbSet.Remove(item);
        SaveChanges();
    }

    public async Task RemoveAsync(int id, CancellationToken cancel = default)
    {
        T item = Get(id) ?? throw new Exception("Элемент не найден.");
        dbSet.Remove(item);
        await SaveChangesAsync(cancel);
    }

    public void RemoveRange(IEnumerable<T> items)
    {
        dbSet.RemoveRange(items);
        SaveChanges();
    }
    public async Task RemoveRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
    {
        dbSet.RemoveRange(items);
        await SaveChangesAsync(cancel);
    }

    private void SaveChanges() => context.SaveChanges();

    private async Task SaveChangesAsync(CancellationToken cancel) => await context.SaveChangesAsync(cancel).ConfigureAwait(false);
}
