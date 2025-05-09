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
using Microsoft.AspNetCore.DataProtection;
using Syncfusion.Blazor;
using Common.Services.Submit;
using BlazorApp;
using CallejoIncChildcareAPI.Filters;
using Microsoft.OpenApi.Models;
using Common.Services.Expenses;
using Common.Services.Invoice;
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

// Add Blazor Server with SignalR configurationvar app
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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<DailyScheduleService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<ISubmitService, SubmitService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddHttpClient();

builder.Services.AddSyncfusionBlazor(); // Adds Syncfusion Blazor Service
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX1cdnZVRGRfUUFwWUE="); //Register Syncfusion license


builder.Services.AddDbContext<CallejoSystemDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Server=.;Database=Callejo_System_DB;Trusted_Connection=True;TrustServerCertificate=True;")));


// Creates a shared encryption key for both the API and Website
// In order for this to work, you need to create the SharedKeys folder in the C: drive
builder.Services.AddDataProtection()
    .SetApplicationName("CallejoIncApp") 
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\SharedKeys\")); // Make sure that path of SharedKeys folder matches this path string



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
    options.AccessDeniedPath = "/UnauthorizedAccess";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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

    // Enable file upload support in Swagger
    options.OperationFilter<SwaggerFileUploadOperationFilter>();

    // Support for authorization in Swagger (if needed)
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

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    // Enable Swagger in development
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