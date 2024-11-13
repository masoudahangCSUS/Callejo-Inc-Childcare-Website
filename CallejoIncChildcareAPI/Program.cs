using Common.Models.Data;
using Common.Services.Role;
using Common.Services.SQL;
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
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISQLServices, SQLServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
