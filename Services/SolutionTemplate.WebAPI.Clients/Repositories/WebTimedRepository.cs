using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.WebAPI.Clients.Repositories
{
    public class WebTimedRepository<T> : WebRepository<T>, ITimedRepository<T> where T : ITimedEntity
    {
        public WebTimedRepository(HttpClient Client, ILogger<WebRepository<T>> Logger) : base(Client, Logger) { }

        public async Task<bool> ExistGreaterThenTime(DateTimeOffset ReferenceTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<bool>($"time/greater({ReferenceTime})/any", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<int> GetCountGreaterThenTime(DateTimeOffset ReferenceTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<int>($"time/greater({ReferenceTime})/count", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> GetAllGreaterThenTime(DateTimeOffset ReferenceTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"time/greater({ReferenceTime})", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> GetGreaterThenTime(DateTimeOffset ReferenceTime, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"time/greater({ReferenceTime})/items[{Skip}:{Count}]", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IPage<T>> GetPageGreaterThenTime(DateTimeOffset ReferenceTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IPage<T>>($"time/greater({ReferenceTime})/page[{PageIndex}/{PageSize}]", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<bool> ExistLessThenTime(DateTimeOffset ReferenceTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<bool>($"time/less({ReferenceTime})/any", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<int> GetCountLessThenTime(DateTimeOffset ReferenceTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<int>($"time/less({ReferenceTime})/count", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> GetAllLessThenTime(DateTimeOffset ReferenceTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"time/less({ReferenceTime})", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> GetLessThenTime(DateTimeOffset ReferenceTime, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"time/less({ReferenceTime})/items[{Skip}:{Count}]", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IPage<T>> GetPageLessThenTime(DateTimeOffset ReferenceTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IPage<T>>($"time/less({ReferenceTime})/page[{PageIndex}/{PageSize}]", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> GetAllInTimeInterval(DateTimeOffset StartTime, DateTimeOffset EndTime, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"time/greater({StartTime})/less({EndTime})", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IPage<T>> GetInTimeInterval(DateTimeOffset StartTime, DateTimeOffset EndTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IPage<T>>($"time/greater({StartTime})/less{EndTime}/page[{PageIndex}/{PageSize}]", Cancel).ConfigureAwait(false);
            return items;
        }
    }
}
