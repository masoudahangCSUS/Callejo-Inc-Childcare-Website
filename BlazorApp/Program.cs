using BlazorApp.Client;
using Common.Models.Data;
using Common.Services.Role;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Client.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for making HTTP requests in Blazor components
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Environment.IsDevelopment()
            ? "https://localhost:7139" // Correct API URL for development
            : builder.Configuration["BaseAddress"])
    });

// Add Controllers for API endpoints
builder.Services.AddControllers();

// Add Blazor Server with SignalR configuration
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(60);
    });

// Register any additional services
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddDbContext<CallejoSystemDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Server=DESKTOP-49NHJ9N;Database=Callejo_System_DB;Trusted_Connection=True;TrustServerCertificate=True;")));

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