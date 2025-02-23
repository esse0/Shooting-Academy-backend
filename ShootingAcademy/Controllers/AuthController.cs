using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShootingAcademy.Models;
using ShootingAcademy.Models.DB.ModelUser;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

//namespace ShootingAcademy.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private static Dictionary<string, string?> RefreshTokens = new Dictionary<string, string>();
//        private readonly IConfiguration _configuration;
//        private readonly ApplicationDbContext _context;

//        public AuthController(IConfiguration configuration, ApplicationDbContext context)
//        {
//            _configuration = configuration;
//            _context = context;
//        }

//        private JwtSecurityToken GenerateAccessToken(User user)
//        {

//            var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Email, user.id),
//            new Claim(ClaimTypes.Role, _roleTrackerService.GetEntrieById(user.roleId).ROLE),
//        };

//            var token = new JwtSecurityToken(
//                issuer: _configuration["JwtSettings:Issuer"],
//                audience: _configuration["JwtSettings:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(20),
//                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
//                    SecurityAlgorithms.HmacSha256)
//            );

//            return token;
//        }

//        [HttpPost("register")]
//        public IActionResult Register([FromBody] LoginForm form)
//        {
//            try
//            {
//                using (MD5 md5Hash = MD5.Create())
//                {
//                    byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(form.Password));
//                    StringBuilder builder = new StringBuilder();
//                    foreach (byte b in bytes)
//                    {
//                        builder.Append(b.ToString("x2"));
//                    }

//                    User.Model user = new User.Model
//                    {
//                        id = form.Email,
//                        password = builder.ToString(),
//                        roleId = _roleTrackerService.GetEntrieByName("user").ID,
//                    };

//                    _userTrackerService.CreateEntry(user);

//                    var token = GenerateAccessToken(user);

//                    var refreshToken = Guid.NewGuid().ToString();

//                    RefreshTokens[refreshToken] = user.id;

//                    return Ok(new AuthResponse
//                    {
//                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
//                        refreshToken = refreshToken,
//                        role = "user",
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new AuthResponse
//                {
//                    error = ex.Message,
//                });
//            }
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginForm form)
//        {
//            try
//            {
//                User.Model user = _userTrackerService.GetEntrie(form);

//                if (user != null)
//                {
//                    var token = GenerateAccessToken(user);

//                    var refreshToken = Guid.NewGuid().ToString();

//                    RefreshTokens[refreshToken] = user.id;

//                    return Ok(new AuthResponse
//                    {
//                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
//                        refreshToken = refreshToken,
//                        role = _roleTrackerService.GetEntrieById(user.roleId).ROLE,
//                    });

//                }
//                return Unauthorized(new AuthResponse
//                {
//                    error = "Неверные данные",
//                });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new AuthResponse
//                {
//                    error = ex.Message,
//                });
//            }
//        }

//        /*
//        [HttpPost("refresh")]
//        public IActionResult Refresh([FromBody] RefreshRequest request)
//        {
//            if (RefreshTokens.TryGetValue(request.RefreshToken, out var userId))
//            {
//                // Generate a new access token
//                var token = GenerateAccessToken(userId);

//                // Return the new access token to the client
//                return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
//            }

//            return BadRequest("Invalid refresh token");
//        }
//        */
//    }
//}
