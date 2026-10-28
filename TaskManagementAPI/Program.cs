using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Persistence.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
//Dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

