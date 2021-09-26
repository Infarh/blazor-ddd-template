using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.WebAPI.Clients.Repositories
{
    public class WebNamedRepository<T> : WebRepository<T>, INamedRepository<T> where T : INamedEntity
    {
        public WebNamedRepository(HttpClient Client, ILogger<WebNamedRepository<T>> Logger) : base(Client, Logger) { }

        public async Task<bool> ExistName(string Name, CancellationToken Cancel = default)
        {
            var response = await _Client.GetAsync($"exist/name/{Name}", Cancel).ConfigureAwait(false);
            return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
        }

        public async Task<T> GetByName(string Name, CancellationToken Cancel = default) =>
            await _Client.GetAsync($"name/{Name}", Cancel).ConfigureAwait(false) switch
            {
                { StatusCode: HttpStatusCode.NotFound } => default,
                { } response => await response
                   .EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<T>(cancellationToken: Cancel)
                   .ConfigureAwait(false)
            };

        public async Task<T> DeleteByName(string Name, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Delete Name:{0}", Name);

            var response = await _Client.DeleteAsync($"name/{Name}", Cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound) return default;
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel)
               .ConfigureAwait(false);
            _Logger.LogInformation("Delete Name:{0} complete. Receive response {1}", Name, (object)result ?? "<null>");
            return result;
        }
    }
}
