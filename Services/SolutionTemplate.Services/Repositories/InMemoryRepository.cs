using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.Services.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity<int>
    {
        private static readonly Action<T, int> __IdSetter = typeof(T).GetProperty(nameof(IEntity.Id))!.SetMethod!.CreateDelegate<Action<T, int>>();

        private readonly IDictionary<int, T> _Items;
        private int _FreeId;

        public InMemoryRepository()
        {
            _Items = new Dictionary<int, T>();
            _FreeId = 1;
        }

        public InMemoryRepository(IEnumerable<T> Items)
        {
            if (Items is null) throw new ArgumentNullException(nameof(Items));

            _Items = Items.ToDictionary(item => item.Id);
            _FreeId = _Items.Keys.DefaultIfEmpty().Max() + 1;
        }

        public Task<bool> IsEmpty(CancellationToken Cancel = default) => Cancel.IsCancellationRequested
            ? Task.FromCanceled<bool>(Cancel)
            : Task.FromResult(_Items.Count == 0);

        public Task<bool> ExistId(int Id, CancellationToken Cancel = default) => Cancel.IsCancellationRequested
            ? Task.FromCanceled<bool>(Cancel)
            : Task.FromResult(_Items.ContainsKey(Id));

        public Task<bool> Exist(T item, CancellationToken Cancel = default) =>
            Cancel.IsCancellationRequested
                ? Task.FromCanceled<bool>(Cancel)
                : Task.FromResult(_Items.ContainsKey(item.Id));

        public Task<int> GetCount(CancellationToken Cancel = default) => Cancel.IsCancellationRequested
            ? Task.FromCanceled<int>(Cancel)
            : Task.FromResult(_Items.Count);

        public Task<IEnumerable<T>> GetAll(CancellationToken Cancel = default) => Cancel.IsCancellationRequested
            ? Task.FromCanceled<IEnumerable<T>>(Cancel)
            : Task.FromResult(_Items.Values.AsEnumerable());

        public Task<IEnumerable<T>> Get(int Skip, int Count, CancellationToken Cancel = default) => Cancel.IsCancellationRequested
            ? Task.FromCanceled<IEnumerable<T>>(Cancel)
            : Task.FromResult(_Items.Values.Skip(Skip).Take(Count));

        public Task<IPage<T>> GetPage(int PageNumber, int PageSize, CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<IPage<T>>(Cancel);

            var items = _Items.Values.Skip(PageNumber * PageSize).Take(PageSize).ToArray();
            IPage<T> page = new Page(items, items.Length, _Items.Count, PageNumber, PageSize);
            return Task.FromResult(page);
        }

        private record Page(IEnumerable<T> Items, int ItemsCount, int TotalCount, int PageNumber, int PageSize) : IPage<T>;

        public Task<T> GetById(int Id, CancellationToken Cancel = default) => Cancel.IsCancellationRequested
            ? Task.FromCanceled<T>(Cancel)
            : Task.FromResult(_Items.TryGetValue(Id, out var item) ? item : default);

        public Task<T> Add(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<T>(Cancel);

            if (item.Id != 0) throw new ArgumentException("У добавляемого элемента установлен идентификатор", nameof(item));

            __IdSetter(item, _FreeId++);
            _Items.Add(item.Id, item);
            return Task.FromResult(item);
        }

        public Task AddRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled(Cancel);

            foreach (var item in items)
            {
                if (Cancel.IsCancellationRequested)
                    return Task.FromCanceled(Cancel);

                __IdSetter(item, _FreeId++);
                _Items.Add(item.Id, item);
            }

            return Task.CompletedTask;
        }

        public Task<T> Update(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<T>(Cancel);

            var id = item.Id;
            if (_Items.ContainsKey(id))
                _Items[id] = item;
            return Task.FromResult(item);
        }

        public Task<T> UpdateAsync(int id, Action<T> ItemUpdated, CancellationToken Cancel = default)
        {
            if (ItemUpdated is null) throw new ArgumentNullException(nameof(ItemUpdated));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<T>(Cancel);

            if (!_Items.ContainsKey(id))
                return Task.FromResult<T>(default);

            var item = _Items[id];
            ItemUpdated(item);

            return Task.FromResult(item);
        }

        public Task UpdateRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled(Cancel);

            foreach (var item in items)
            {
                if (Cancel.IsCancellationRequested)
                    return Task.FromCanceled(Cancel);

                var id = item.Id;
                if(!_Items.ContainsKey(id))
                    continue;
                _Items[id] = item;
            }

            return Task.CompletedTask;
        }

        public Task<T> Delete(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<T>(Cancel);

            var id = item.Id;
            if (!_Items.TryGetValue(id, out var deleted_item))
                return Task.FromResult<T>(default);
            _Items.Remove(id);
            return Task.FromResult(deleted_item);
        }

        public Task DeleteRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));

            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled(Cancel);

            foreach (var item in items)
            {
                if (Cancel.IsCancellationRequested)
                    return Task.FromCanceled(Cancel);

                var id = item.Id;
                if (!_Items.ContainsKey(id))
                    continue;
                _Items.Remove(id);
            }

            return Task.CompletedTask;
        }

        public Task<T> DeleteById(int id, CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<T>(Cancel);

            if (!_Items.TryGetValue(id, out var deleted_item))
                return Task.FromResult<T>(default);
            _Items.Remove(id);
            return Task.FromResult(deleted_item);
        }
    }
}
