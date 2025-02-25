using BlazorApp.Client;
using Common.Models.Data;
using Common.Services.Role;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Common.Services.SQL;
using Common.Services.User;


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
builder.Services.AddScoped<ISQLServices, SQLServices>();
builder.Services.AddScoped<HolidaysVacationsService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddDbContext<CallejoSystemDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Server=.;Database=Callejo_System_DB;Trusted_Connection=True;TrustServerCertificate=True;")));

builder.Services.AddAuthentication(options =>
{
    // Set the default schemes for authentication, challenge, and sign in.
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Configure cookie settings as needed.
    options.Cookie.Name = "MyAppAuthCookie";
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
// Load environment variables
Env.Load();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
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