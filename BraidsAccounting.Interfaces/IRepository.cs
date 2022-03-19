using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Interfaces
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        public IQueryable<T> Items { get;  }
        public T? Get(int id);
        public Task<T?> GetAsync(int id, CancellationToken cancel = default);
        public T Create(T item);
        public Task<T?> CreateAsync(T item, CancellationToken cancel = default);
        public void CreateRange(IEnumerable<T> items);
        public Task CreateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);
        public void Edit(T item);
        public Task EditAsync(T item, CancellationToken cancel = default);
        public void EditRange(IEnumerable<T> item);
        public Task EditRangeAsync(IEnumerable<T> item, CancellationToken cancel = default);
        public void Remove(int id);
        public Task RemoveAsync(int id, CancellationToken cancel = default);
        public void RemoveRange(IEnumerable<T> items);
        public Task RemoveRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);

    }
}
