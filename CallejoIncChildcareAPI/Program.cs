using Common.Models.Data;
using Common.Services.Role;
using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

// Register services here
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISQLServices, SQLServices>();
builder.Services.AddScoped<IUserService, UserService>();
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
        policy.WithOrigins("https://localhost:7273") // The origin of your Blazor app
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});




var app = builder.Build();

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
