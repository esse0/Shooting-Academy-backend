using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ShootingAcademy.Middleware;
using ShootingAcademy.Models;
using ShootingAcademy.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();

var dataSource = dataSourceBuilder.Build();
builder.Services.AddSingleton(dataSource);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(dataSource);
});

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("Coors",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtManager.GetParameters(access);
    });

builder.Services.AddSingleton(new JwtManager(access, refresh));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseGetToken();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
app.UseCors("Coors");

app.UseSession();


app.MapFallbackToFile("index.html");

app.Run();
