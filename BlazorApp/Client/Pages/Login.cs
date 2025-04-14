using BlazorApp.Client.Services;
using Common.View;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;


namespace BlazorApp.Client.Pages
{
    public partial class Login : ComponentBase
    {
        private LoginDTO loginInfo = new();
        private bool loginState = true;
        private string errorMessage = string.Empty;

        private async Task HandleLogin()
        {
            try
            {
                APIResponse response = await LoginService.LoginUser(loginInfo.Email, loginInfo.Password);

                
                if (response.Success)
                {
                    UserSession.SetUser(true, response.RoleId, Guid.Parse(response.Token.ToString()), response.Role); // Store user session information

                    // Notify the authentication state provider
                    AuthenticationStateProvider.NotifyUserAuthentication(Guid.Parse(response.Token.ToString()), response.RoleId, response.Role);

                    if (response.RoleId == 1)
                    {
                        Console.WriteLine("DEBUG: Navigating to /admin");
                        await InvokeAsync(() => Navigation.NavigateTo("/admin", forceLoad: true));

                    }
                    else
                    {
                        Console.WriteLine("DEBUG: Navigating to /");
                        await InvokeAsync(() => Navigation.NavigateTo("/", forceLoad: true));
                    }
                }
                else
                {
                    loginState = false;
                    errorMessage = response.Message ?? "Login failed or no data returned.";
                }
            }
            catch (Exception ex) // Exception usually occurs if backend is not running
            {
                loginState = false;
                errorMessage = "An error occurred. Please try again.";
                Console.WriteLine($"DEBUG: Login error? {ex.Message}");
            }
        }
    }
}
