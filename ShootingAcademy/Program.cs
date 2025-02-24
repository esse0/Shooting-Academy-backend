using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShootingAcademy.Middleware;
using ShootingAcademy.Models;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Services;
using System.Data.Common;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

//Add JWT authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = configuration["JwtSettings:Issuer"],
//            ValidAudience = configuration["JwtSettings:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
//        };
//    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddTransient<IUserService, UserService>();

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
