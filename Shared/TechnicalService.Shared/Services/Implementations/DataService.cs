using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Shared.Services.Implementations
{
    public class DataService<T> : IDataService<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient(string clientType)
        {
            var client = _httpClientFactory.CreateClient(clientType);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<T> GetAsync(string endpoint, string clientType)
        {
            try
            {
                var client = CreateClient(clientType);
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                var result = await client.SendAsync(request);
                return await result.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception)
            {
                throw new Exception("Beklenmedik bir hata oluştu");
            }

        }
        public async Task<IEnumerable<T>> GetListAsync(string endpoint, string clientType)
        {
            var client = CreateClient(clientType);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var result = await client.SendAsync(request);
            return await result.Content.ReadFromJsonAsync<IEnumerable<T>>();
        }
        public async Task<HttpResponseMessage> CreateAsync(string endpoint, T data, string clientType)
        {
            var client = CreateClient(clientType);
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint) { Content = JsonContent.Create(data) };
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var result = await client.SendAsync(request);
            return result;
        }
        public async Task<HttpResponseMessage> UpdateAsync(string endpoint, T data, string clientType)
        {
            var client = CreateClient(clientType);
            var request = new HttpRequestMessage(HttpMethod.Put, endpoint) { Content = JsonContent.Create(data) };
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var result = await client.SendAsync(request);

            return result;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, string clientType)
        {
            var client = CreateClient(clientType);
            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var result = await client.SendAsync(request);
            return result;
        }
    }
}


