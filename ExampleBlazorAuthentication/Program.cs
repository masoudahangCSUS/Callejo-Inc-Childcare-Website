using ExampleBlazorAuthentication;
using ExampleBlazorAuthentication.Components;
using ExampleBlazorAuthentication.Service;
using Microsoft.AspNetCore.Components.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Populate the AppSettings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApiSettings"));

// Register Services
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Register IHttpContextAccessor for dependency injection

builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

// Middleware to set the authentication cookie
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Cookies.Delete("AuthGUID"); // Clear any existing authentication cookie
        context.Response.Cookies.Delete("AuthUserName"); // Clear any existing authentication cookie
    }
    await next.Invoke();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
