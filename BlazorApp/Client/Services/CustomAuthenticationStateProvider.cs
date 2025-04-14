using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Client.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly UserSessionService _userSessionService;

        public CustomAuthenticationStateProvider(UserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
        }

        public bool isOwner()
        {
            if (_userSessionService.UserIsLoggedIn && _userSessionService.UserRoleName.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public bool isEmployee()
        {
            if (_userSessionService.UserIsLoggedIn && _userSessionService.UserRoleName.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public bool isGuardian()
        {
            if (_userSessionService.UserIsLoggedIn && _userSessionService.UserRoleName.Equals("Guardian", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = _userSessionService.UserIsLoggedIn
                ? new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, _userSessionService.AuthToken.ToString()),
                    new Claim(ClaimTypes.Role, (_userSessionService.UserRole == null ? string.Empty : _userSessionService.UserRoleName)),
                    new Claim("RoleId", _userSessionService.UserRole.ToString())
                }, "CustomAuth")
                : new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        public void NotifyUserAuthentication(Guid authToken, long roleId, string roleName)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, authToken.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("RoleId", roleId.ToString())
            }, "CustomAuth");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
