using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace SolutionTemplate.WebAPI.Clients.Repositories
{
    public class WebRepository<T> : IRepository<T> where T : IEntity
    {
        protected readonly HttpClient _Client;
        protected readonly ILogger<WebRepository<T>> _Logger;

        public WebRepository(HttpClient Client, ILogger<WebRepository<T>> Logger)
        {
            _Client = Client;
            _Logger = Logger;
        }

        public async Task<bool> IsEmpty(CancellationToken Cancel = default)
        {
            var result = await _Client.GetFromJsonAsync<bool>("isempty", Cancel).ConfigureAwait(false);
            return result;
        }

        public async Task<int> GetCount(CancellationToken Cancel = default)
        {
            var count = await _Client.GetFromJsonAsync<int>("count", Cancel).ConfigureAwait(false);
            return count;
        }

        public async Task<bool> ExistId(int Id, CancellationToken Cancel = default)
        {
            var response = await _Client.GetAsync($"exist/{Id}", Cancel).ConfigureAwait(false);
            return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
        }

        public async Task<bool> Exist(T item, CancellationToken Cancel = default)
        {
            var response = await _Client.PostAsJsonAsync("exist", item, Cancel).ConfigureAwait(false);
            return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>("", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> Get(int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"items[{Skip}:{Count}]", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IPage<T>> GetPage(int PageNumber, int PageSize, CancellationToken Cancel = default) =>
            await _Client.GetAsync($"page[{PageNumber}/{PageSize}]", HttpCompletionOption.ResponseHeadersRead, Cancel).ConfigureAwait(false) switch
            {
                { StatusCode: HttpStatusCode.NotFound } response => await response
                   .Content
                   .ReadFromJsonAsync<PagedItems>(cancellationToken: Cancel)
                   .ConfigureAwait(false),
                { } response => await response.EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<PagedItems>(cancellationToken: Cancel)
                   .ConfigureAwait(false),
                _ => throw new InvalidOperationException()
            };

        protected record PagedItems(IEnumerable<T> Items, int ItemsCount, int TotalCount, int PageNumber, int PageSize) : IPage<T>;

        public async Task<T> GetById(int Id, CancellationToken Cancel = default) =>
            await _Client.GetAsync($"{Id}", HttpCompletionOption.ResponseHeadersRead, Cancel).ConfigureAwait(false) switch
            {
                { StatusCode: HttpStatusCode.NotFound } => default,
                { } response => await response.EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<T>(cancellationToken: Cancel)
                   .ConfigureAwait(false)
            };

        public async Task<T> Add(T item, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Add {0}", item);
            var response = await _Client.PostAsJsonAsync("", item, Cancel).ConfigureAwait(false);
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Add {0} complete. Receive response {1}", item, (object)result ?? "<null>");
            return result;
        }

        public async Task AddRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            var response = await _Client.PostAsJsonAsync("items", items, Cancel).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> Update(T item, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Update {0}", item);
            var response = await _Client.PutAsJsonAsync("", item, Cancel).ConfigureAwait(false);
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Update {0} complete. Receive response {1}", item, (object)result ?? "<null>");
            return result;
        }

        public async Task UpdateRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            var response = await _Client.PutAsJsonAsync("items", items, Cancel).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> Delete(T item, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Delete {0}", item);
            var request = new HttpRequestMessage(HttpMethod.Delete, "")
            {
                Content = JsonContent.Create(item)
            };
            var response = await _Client.SendAsync(request, Cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _Logger.LogInformation("Delete {0} - item not exist", item);
                return default;
            }
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Delete {0} complete. Receive response {1}", item, (object)result ?? "<null>");
            return result;
        }

        public async Task DeleteRange(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "items")
            {
                Content = JsonContent.Create(items)
            };
            var response = await _Client.SendAsync(request, Cancel).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> DeleteById(int id, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Delete id:{0}", id);
            var response = await _Client.DeleteAsync($"{id}", Cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _Logger.LogInformation("Delete id:{0} - item not exist", id);
                return default;
            }
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Delete id:{0} complete. Receive response {1}", id, (object)result ?? "<null>");
            return result;
        }
    }
}
