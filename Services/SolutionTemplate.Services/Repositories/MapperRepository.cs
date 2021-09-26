using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.Services.Repositories
{
    public class MapperRepository<T, TEntity> : IRepository<T> where T : IEntity where TEntity : IEntity
    {
        private readonly IRepository<TEntity> _SourceRepository;
        private readonly IMapper _Mapper;

        public MapperRepository(IRepository<TEntity> SourceRepository, IMapper Mapper)
        {
            _SourceRepository = SourceRepository;
            _Mapper = Mapper;
        }

        public async Task<bool> ExistId(int Id, CancellationToken Cancel = default) =>
            await _SourceRepository.ExistId(Id, Cancel).ConfigureAwait(false);

        public async Task<bool> Exist(T item, CancellationToken Cancel = default)
        {
            var entity = _Mapper.Map<TEntity>(item);
            var is_exist = await _SourceRepository.Exist(entity, Cancel).ConfigureAwait(false);
            return is_exist;
        }

        public async Task<int> GetCount(CancellationToken Cancel = default) =>
            await _SourceRepository.GetCount(Cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAll(CancellationToken Cancel = default)
        {
            var items = await _SourceRepository.GetAll(Cancel).ConfigureAwait(false);
            return items.Select(item => _Mapper.Map<T>(item));
        }

        public async Task<IEnumerable<T>> Get(int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await _SourceRepository.Get(Skip, Count, Cancel).ConfigureAwait(false);
            return items.Select(item => _Mapper.Map<T>(item));
        }

        public async Task<IPage<T>> GetPage(int PageNumber, int PageSize, CancellationToken Cancel = default)
        {
            var page = await _SourceRepository.GetPage(PageNumber, PageSize, Cancel).ConfigureAwait(false);
            return new Page(page.Items.Select(item => _Mapper.Map<T>(item)), page.ItemsCount, page.TotalCount, page.PageNumber, page.PageSize);
        }

        private record Page(IEnumerable<T> Items, int ItemsCount, int TotalCount, int PageNumber, int PageSize) : IPage<T>;

        public async Task<T> GetById(int Id, CancellationToken Cancel = default)
        {
            var item = await _SourceRepository.GetById(Id, Cancel).ConfigureAwait(false);
            return _Mapper.Map<T>(item);
        }

        public async Task<T> Add(T item, CancellationToken Cancel = default)
        {
            var entity = _Mapper.Map<TEntity>(item);
            if (await _SourceRepository.Add(entity, Cancel).ConfigureAwait(false) is not { } added_entity)
                return default;
            var added_item = _Mapper.Map<T>(added_entity);
            return added_item;
        }

        public async Task AddRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            var entities = items.Select(item => _Mapper.Map<TEntity>(item));
            await _SourceRepository.AddRange(entities, Cancel).ConfigureAwait(false);
        }

        public async Task<T> Update(T item, CancellationToken Cancel = default)
        {
            var entity = _Mapper.Map<TEntity>(item);
            if (await _SourceRepository.Update(entity, Cancel).ConfigureAwait(false) is not { } updated_entity)
                return default;
            var updated_item = _Mapper.Map<T>(updated_entity);
            return updated_item;
        }

        public async Task UpdateRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            var entities = items.Select(item => _Mapper.Map<TEntity>(item));
            await _SourceRepository.UpdateRange(entities, Cancel).ConfigureAwait(false);
        }

        public async Task<T> Delete(T item, CancellationToken Cancel = default)
        {
            var entity = _Mapper.Map<TEntity>(item);
            if (await _SourceRepository.Delete(entity, Cancel).ConfigureAwait(false) is not { } deleted_entity)
                return default;
            var deleted_item = _Mapper.Map<T>(deleted_entity);
            return deleted_item;
        }

        public async Task DeleteRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            var entities = items.Select(item => _Mapper.Map<TEntity>(item));
            await _SourceRepository.DeleteRange(entities, Cancel).ConfigureAwait(false);
        }

        public async Task<T> DeleteById(int id, CancellationToken Cancel = default)
        {
            if (await _SourceRepository.DeleteById(id, Cancel).ConfigureAwait(false) is not { } deleted_entity) 
                return default;
            var deleted_item = _Mapper.Map<T>(deleted_entity);
            return deleted_item;
        }
    }
}
