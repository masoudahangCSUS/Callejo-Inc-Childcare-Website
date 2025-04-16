using BlazorApp.Client;
//using Common.Models.Data;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using BlazorApp.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Syncfusion.Blazor;
using BlazorApp;
using CallejoIncChildcareAPI.Filters;
using Microsoft.OpenApi.Models;
using Services.Submit;
using Services.Invoice;
using Services.Expenses;
using CallejoIncChildCareAPI.Authentication;
using CallejoIncChildCareAPI.Authorize;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApiSettings"));

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
builder.Services.AddScoped<HolidaysVacationsService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<DailyScheduleService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ISubmitService, SubmitService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddSyncfusionBlazor(); // Adds Syncfusion Blazor Service
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX1cdnZVRGRfUUFwWUE="); // Register Syncfusion license

// Creates a shared encryption key for both the API and Website
builder.Services.AddDataProtection()
    .SetApplicationName("CallejoIncApp")
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\SharedKeys\")); // Make sure that path of SharedKeys folder matches this path string


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//.AddCookie(options =>
//{
//    options.Cookie.Name = "MyAppAuthCookie";
//    options.LoginPath = "/Login";
//    options.AccessDeniedPath = "/UnauthorizedAccess";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
//    options.SlidingExpiration = true;
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//});

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


// Load environment variables
Env.Load();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Callejo API",
        Version = "v1",
        Description = "API documentation for Callejo Inc Childcare system, including file upload support."
    });

    options.OperationFilter<SwaggerFileUploadOperationFilter>();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}' to authenticate."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Populate the AppSettings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApiSettings"));

// Register HttpClient for making HTTP requests in Blazor components
builder.Services.AddScoped(sp =>
{
    var appSettings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
    return new HttpClient
    {
        BaseAddress = new Uri(appSettings.BaseUrl)
    };
});

var app = builder.Build();

// Load the configuration into the AppSettings object
var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Callejo API V1");
    });
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