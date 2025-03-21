using CallejoIncChildCareAPI;
using CallejoIncChildCareAPI.Authentication;
using CallejoIncChildCareAPI.Authorize;
using Common.Models.Data;
using Common.Services.DailySchedule;
using Common.Services.Login;
using Common.Services.Registration;
using Common.Services.Role;
using Common.Services.SQL;
using Common.Services.Submit;
using Common.Services.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// This statement sets up the API so that it will use the model defined in the common library
// This line must come before the instruction 
// var app = builder.Build();
builder.Services.AddDbContext<CallejoSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApiSettings"));


// Register services here
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<IDailyScheduleService, DailyScheduleService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISQLServices, SQLServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubmitService, SubmitService>();
builder.Services.AddScoped<IRegService, RegService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddHttpClient();


// Creates a shared encryption key for both the API and Website
// In order for this to work, you need to create the SharedKeys folder in your C: drive
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
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});



builder.Services.AddControllersWithViews();

// Add CORS services and configure a policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7273") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});



var app = builder.Build();

// Load the configuration into the AppSettings object
var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
AuthenticateAction.Key = appSettings.Key.ToString(); // Set the key for the AuthenticateAction class
AuthenticateAction.Applications = appSettings.Applications; // Set the applications for the AuthenticateAction class
AuthorizeAction.Applications = appSettings.Applications; // Set the applications for the AuthorizeAction class
AuthorizeAction.Key = appSettings.Key.ToString(); // Set the key for the AuthorizeAction class


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
}
// Apply the CORS policy here.
app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

app.Run();
