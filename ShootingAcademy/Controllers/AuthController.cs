using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.Auth;
using ShootingAcademy.Models.Errors;
using ShootingAcademy.Services;

namespace ShootingAcademy.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtManager _jwtManager;
        private readonly PasswordHasher _passwordHasher;
        private readonly ApplicationDbContext _db;

        public AuthController(JwtManager jwtManager, PasswordHasher passwordHasher, ApplicationDbContext db)
        {
            _jwtManager = jwtManager;
            _passwordHasher = passwordHasher;
            _db = db;
        }

        [HttpPost("signin")]
        public async Task<IResult> signinJwt([FromBody] SigninModel model)
        {
            try
            {
                var user = await _db.Users.FirstAsync(i => i.Email == model.email);

                if (!_passwordHasher.Verify(model.password, user.PasswordHash))
                {
                    throw new ApplicationException("Пароли не совпадают");
                }

                HttpContext.Response.Cookies.Append(
                    "AccessToken",
                    JwtManager.GenerateJwtToken(_jwtManager.AccessToken, user),
                    _jwtManager.JwtCookieOptions
                );

                string token = JwtManager.GenerateJwtToken(_jwtManager.RefreshToken, user);

                HttpContext.Response.Cookies.Append(
                    "RefreshToken",
                    token,
                    _jwtManager.JwtCookieOptions
                );

                //user.Token = token;

                _db.Users.Update(user);

                await _db.SaveChangesAsync();

                return Results.Ok();
            }
            catch (ApplicationException apperr)
            {
                return Results.Json(new BaseError()
                {
                    Show = true,
                    Code = "400",
                    Error = true,
                    Message = apperr.Message
                }, statusCode: 400);
            }
            catch (Exception err)
            {
                return Results.Problem(err.Message, statusCode: 400);
            }
        }

        [HttpGet("signout")]
        public IResult logoutJwt()
        {
            HttpContext.Response.Cookies.Delete("AccessToken");
            HttpContext.Response.Cookies.Delete("RefreshToken");

            return Results.Ok();
        }

        [HttpPost("register")]
        public async Task<IResult> Register([FromForm] RegisterModel model)
        {
            Console.WriteLine(model.email);

            try
            {
                var usr = await _db.Users.AddAsync(new()
                {
                    FirstName = model.name,
                    SecoundName = model.lastName,
                    PatronymicName = string.Empty,
                    Email = model.email,
                    PasswordHash = _passwordHasher.Hash(model.password),
                    Role = "Athlete",
                    Grade = "",
                    Age = 0,
                    Country = ""
                });

                await _db.SaveChangesAsync();
            }
            catch
            {
                return Results.Json(new BaseError()
                {
                    Show = true,
                    Code = "400",
                    Error = true,
                    Message = "Some error!"
                }, statusCode: 400);
            }

            return Results.Ok();
        }
    }
}
