using IsPrimeNumber.Infrastructure.Middlewares;
using IsPrimeNumber.Infrastructure.Persistence;
using IsPrimeNumber.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<INumberService, NumberService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
