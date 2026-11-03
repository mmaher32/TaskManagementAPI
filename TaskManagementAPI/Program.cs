using Microsoft.EntityFrameworkCore;
using TaskManagement.API.EndPoints;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Persistence.DbContexts;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHash>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();
app.MapAuthEndPoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "TaskManagment API");
    });
}

app.UseHttpsRedirection();

app.Run();

