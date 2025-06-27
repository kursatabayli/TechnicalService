using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Shared.AuthHelpers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public CustomAuthStateProvider(IAuthService authService, NavigationManager navigationManager)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }

        private UserClaims? _cachedUserClaims;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_cachedUserClaims != null)
                return CreateAuthenticatedState(_cachedUserClaims);

            string httpClientName = GetClientType();
            var userClaims = await _authService.CheckSessionAsync(httpClientName);
            if (userClaims != null)
            {
                _cachedUserClaims = userClaims;
                return CreateAuthenticatedState(userClaims);
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private static AuthenticationState CreateAuthenticatedState(UserClaims user)
        {
            var claims = new List<Claim>
                {
                  new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                  new(ClaimTypes.Email, user.Email),
                  new(ClaimTypes.Name, user.Name),
                  new(ClaimTypes.Surname, user.LastName),
                  new(ClaimTypes.Role, user.Role),
                };

            var identity = new ClaimsIdentity(claims, authenticationType: "ServerAuth");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void UpdateAuthState() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        public void NotifyUserLoggedOut()
        {
            _cachedUserClaims = null;
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        private string GetClientType()
        {
            var baseUri = new Uri(_navigationManager.BaseUri);
            string clientTypes;

            if (baseUri.Port == Hosts.portalPort)
                clientTypes = ClientTypes.PersonnelAuthClient;
            else
                clientTypes = ClientTypes.UserAuthClient;

            //if (baseUri.Host == Hosts.portal)
            //    clientTypes = ClientTypes.PersonnelAuthClient;
            //else
            //    clientTypes = ClientTypes.UserAuthClient;

            return clientTypes;
        }
    }
}