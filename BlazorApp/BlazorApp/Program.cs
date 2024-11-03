using BlazorApp.Components;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for making HTTP requests in Blazor components
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Environment.IsDevelopment() ? "https://localhost:44343" : builder.Configuration["BaseAddress"]) });

// Add Controllers for API endpoints
builder.Services.AddControllers();

builder.Services.AddSingleton<UserSessionService>();


Env.Load();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
