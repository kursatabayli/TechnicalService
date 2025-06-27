using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Shared.Services.Implementations
{
    internal class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
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
        public async Task<Result> LoginAsync(LoginDto loginDto, string loginTokenEndpoint, string clientTypes)
        {
            try
            {
                var client = CreateClient(clientTypes);
                var request = new HttpRequestMessage(HttpMethod.Post, loginTokenEndpoint)
                {
                    Content = JsonContent.Create(loginDto)
                };

                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var result = await client.SendAsync(request);

                return await result.Content.ReadFromJsonAsync<Result>();
            }
            catch (Exception)
            {
                return Result.Failure("Beklenmedik bir hata oluştu.", StatusCode.BadRequest);
            }
        }
        public async Task<Result> RefreshTokenAsync(string refreshTokenEndpoint, string clientTypes)
        {
            try
            {
                var client = CreateClient(clientTypes);

                var request = new HttpRequestMessage(HttpMethod.Post, refreshTokenEndpoint);
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var result = await client.SendAsync(request);

                return await result.Content.ReadFromJsonAsync<Result>();
            }
            catch (Exception)
            {
                return Result.Failure("Beklenmedik bir hata oluştu.", StatusCode.BadRequest);
            }

        }
        public async Task<Result> LogoutAsync(string logoutTokenEndpoint, string clientTypes)
        {
            try
            {
                var client = CreateClient(clientTypes);
                var request = new HttpRequestMessage(HttpMethod.Post, logoutTokenEndpoint);
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                var result = await client.SendAsync(request);

                return await result.Content.ReadFromJsonAsync<Result>();
            }
            catch (Exception)
            {
                return Result.Failure("Beklenmedik bir hata oluştu.", StatusCode.BadRequest);
            }
        }
        public async Task<Result> RegisterUserAsync(RegisterDto registerUser, string clientTypes)
        {
            try
            {
                var client = CreateClient(clientTypes);
                var result = await client.PostAsJsonAsync(Endpoints.Register, registerUser);
                return await result.Content.ReadFromJsonAsync<Result>();
            }
            catch (Exception)
            {
                return Result.Failure("Beklenmedik bir hata oluştu.", StatusCode.BadRequest);
            }
        }
        public async Task<UserClaims> CheckSessionAsync(string clientTypes)
        {
            var client = CreateClient(clientTypes);
            var request = new HttpRequestMessage(HttpMethod.Get, Endpoints.CheckSession);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<UserClaims>();
            }
            return null;
        }
    }
}

