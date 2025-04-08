using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ExampleBlazorAuthentication.Service
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly string _authTokenKey = "authToken";

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _authTokenKey);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // Retrieve the user name from local storage or another source
                var userName = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authUserName");

                var claims = new List<Claim>
                {
                    new Claim("AuthUserName", userName),
                    new Claim("AuthGUID", token)
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void MarkUserAsAuthenticated(string token, string userName)
        {
            var claims = new List<Claim>
        {
            new Claim("AuthUserName", userName),
            new Claim("AuthGUID", token)
        };

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public async ValueTask DisposeAsync()
        {
            // Dispose of any resources here
            await Task.CompletedTask;
        }
    }

}
