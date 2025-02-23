using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShootingAcademy.Models.Controllers.Auth;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Models.DB.ModelUser.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShootingAcademy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private static Dictionary<string, string> RefreshTokens = new Dictionary<string, string>();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        private JwtSecurityToken GenerateAccessToken(User user)
        {

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                    SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO form)
        {
            try
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(form.PasswordHash));
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }

                    User? user = await _userService.AddUserAsync(form); 

                    var token = GenerateAccessToken(user);

                    var refreshToken = Guid.NewGuid().ToString();

                    RefreshTokens[refreshToken] = user.Id.ToString();

                    return Ok(new AuthResponse
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshToken = refreshToken,
                        role = user.Role.Name,
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    error = ex.Message,
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] GetUserDto form)
        {
            try
            {
                User? user = await _userService.FindUserByIdAsync(form);

                if (user != null)
                {
                    var token = GenerateAccessToken(user);

                    var refreshToken = Guid.NewGuid().ToString();

                    RefreshTokens[refreshToken] = user.Id.ToString();

                    return Ok(new AuthResponse
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshToken = refreshToken,
                        role = user.Role.Name,
                    });

                }
                return Unauthorized(new AuthResponse
                {
                    error = "Íåâåðíûå äàííûå",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    error = ex.Message,
                });
            }
        }

        /*
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            if (RefreshTokens.TryGetValue(request.RefreshToken, out var userId))
            {
                // Generate a new access token
                var token = GenerateAccessToken(userId);

                // Return the new access token to the client
                return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest("Invalid refresh token");
        }
        */
    }
}
