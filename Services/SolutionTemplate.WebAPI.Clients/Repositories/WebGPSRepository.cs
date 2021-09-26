using System.Collections.Generic;
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
    public class WebGPSRepository<T> : WebRepository<T>, IGPSRepository<T> where T : IGPSEntity
    {
        public WebGPSRepository(HttpClient Client, ILogger<WebGPSRepository<T>> Logger) : base(Client, Logger) { }

        public async Task<bool> ExistInLocation(double Latitude, double Longitude, double RangeInMeters, CancellationToken Cancel = default)
        {
            var result = await _Client.GetFromJsonAsync<bool>($"exist/GPS({Latitude}:{Longitude})/range({RangeInMeters})", Cancel).ConfigureAwait(false);
            return result;
        }

        public async Task<int> GetCountInLocation(double Latitude, double Longitude, double RangeInMeters, CancellationToken Cancel = default)
        {
            var result = await _Client.GetFromJsonAsync<int>($"count/GPS({Latitude}:{Longitude})/range({RangeInMeters})", Cancel).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<T>> GetAllByLocationInRange(double Latitude, double Longitude, double RangeInMeters, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"GPS({Latitude}:{Longitude})/range({RangeInMeters})", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IEnumerable<T>> GetAllByLocationInRange(double Latitude, double Longitude, double RangeInMeters, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<IEnumerable<T>>($"GPS({Latitude}:{Longitude})/range({RangeInMeters})/items[{Skip}:{Count}]", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<T> GetByLocation(double Latitude, double Longitude, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<T>($"GPS({Latitude}:{Longitude})/closest", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<T> GetByLocationInRange(double Latitude, double Longitude, double RangeInMeters, CancellationToken Cancel = default)
        {
            var items = await _Client.GetFromJsonAsync<T>($"GPS({Latitude}:{Longitude})/range({RangeInMeters})/closest", Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task<IPage<T>> GetPageByLocationInRange(double Latitude, double Longitude, double RangeInMeters, int PageNumber, int PageSize, CancellationToken Cancel = default)
        {
            var page = await _Client.GetFromJsonAsync<IPage<T>>($"GPS({Latitude}:{Longitude})/range({RangeInMeters})/page[{PageNumber}/{PageSize}]", Cancel).ConfigureAwait(false);
            return page;
        }

        public async Task<T> DeleteByLocation(double Latitude, double Longitude, CancellationToken Cancel = default)
        {
            var response = await _Client.DeleteAsync($"GPS({Latitude}:{Longitude})/closest", Cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _Logger.LogInformation("Delete in location:({0}:{1}) - item not exist", Latitude, Longitude);
                return default;
            }
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Delete in location:({0}:{1}) complete. Receive response {2}",
                Latitude, Longitude, 
                (object)result ?? "<null>");
            return result;
        }

        public async Task<T> DeleteByLocationInRange(double Latitude, double Longitude, double RangeInMeters, CancellationToken Cancel = default)
        {
            var response = await _Client.DeleteAsync($"GPS({Latitude}:{Longitude})/range({RangeInMeters})", Cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _Logger.LogInformation("Delete in location:({0}:{1}) range {2} m - item not exist", Latitude, Longitude, RangeInMeters);
                return default;
            }
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<T>(cancellationToken: Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Delete in location:({0}:{1}) range {2} complete. Receive response {2}",
                Latitude, Longitude, RangeInMeters,
                (object)result ?? "<null>");
            return result;
        }
    }
}
