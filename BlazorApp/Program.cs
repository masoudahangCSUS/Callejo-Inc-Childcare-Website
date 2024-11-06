using BlazorApp.Client;
using BlazorApp.Server.Data;
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

builder.Services.AddDbContext<CallejoSystemDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,                 // Number of retry attempts
            maxRetryDelay: TimeSpan.FromSeconds(10), // Delay between retries
            errorNumbersToAdd: null           // Specific SQL error codes to consider transient
        ))
       .EnableSensitiveDataLogging()
       .LogTo(Console.WriteLine)
);

// Add Blazor Server with SignalR configuration
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(60);
    });

// Register any additional services
builder.Services.AddSingleton<UserSessionService>();

// Load environment variables
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
