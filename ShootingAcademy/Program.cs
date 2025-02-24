using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Middleware;
using ShootingAcademy.Models;
using ShootingAcademy.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new PasswordHasher("ALEXYS"));

JwtSettings access = new JwtSettings()
{
    Audience = configuration["JwtSettings:Audience"],
    Issuer = configuration["JwtSettings:Issuer"],
    SecretKey = configuration["JwtSettings:SecretKey"],
    ExpiryMinutes = 10,
};

JwtSettings refresh = new JwtSettings()
{
    Audience = configuration["JwtSettings:Audience"],
    Issuer = configuration["JwtSettings:Issuer"],
    SecretKey = configuration["JwtSettings:SecretKey"],
    ExpiryMinutes = 60,
};

builder.Services.AddSingleton(new JwtManager(access, refresh));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseGetToken();

app.MapFallbackToFile("index.html");

app.Run();
