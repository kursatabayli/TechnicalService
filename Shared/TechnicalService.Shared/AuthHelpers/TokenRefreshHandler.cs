using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Shared.AuthHelpers
{
    public sealed class TokenRefreshHandler : DelegatingHandler
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public TokenRefreshHandler(IAuthService authService, NavigationManager navigationManager)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    await _semaphore.WaitAsync(cancellationToken);

                    var (refreshEndpoint, clientTypes) = GetRefreshTokenEndpoint();

                    var refreshResult = await _authService.RefreshTokenAsync(refreshEndpoint, clientTypes);
                    if (refreshResult.IsSuccess)
                    {
                        var clonedRequest = await CloneRequestAsync(request);
                        return await base.SendAsync(clonedRequest, cancellationToken);
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return response;
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
        {
            var clone = new HttpRequestMessage(original.Method, original.RequestUri)
            {
                Content = original.Content != null
                    ? new StreamContent(await original.Content.ReadAsStreamAsync())
                    : null
            };

            foreach (var header in original.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (original.Content != null)
                foreach (var contentHeader in original.Content.Headers)
                    clone.Content?.Headers.TryAddWithoutValidation(contentHeader.Key, contentHeader.Value);

            clone.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            return clone;
        }

        private (string, string) GetRefreshTokenEndpoint()
        {
            var baseUri = new Uri(_navigationManager.BaseUri);
            string refreshTokenEndpoint, clientTypes;

            //if (baseUri.Host == Hosts.portal)
            //{
            //    refreshTokenEndpoint = Endpoints.PersonnelRefreshToken;
            //    clientTypes = ClientTypes.PersonnelPublicClient;
            //}
            //else
            //{
            //    refreshTokenEndpoint = Endpoints.UserRefreshToken;
            //    clientTypes = ClientTypes.UserPublicClient;
            //}

            if (baseUri.Port == Hosts.portalPort)
            {
                refreshTokenEndpoint = Endpoints.PersonnelRefreshToken;
                clientTypes = ClientTypes.PersonnelPublicClient;
            }
            else
            {
                refreshTokenEndpoint = Endpoints.UserRefreshToken;
                clientTypes = ClientTypes.UserPublicClient;
            }

            return (refreshTokenEndpoint, clientTypes);
        }
    }
}
