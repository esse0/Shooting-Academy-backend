using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.User;
using ShootingAcademy.Models.Exceptions;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private Guid UserGuid => Guid.Parse(HttpContext.User.Claims.ToArray()[0].Value);
        private bool IsAutorize => HttpContext.User.Claims.Any();
        private string Role => HttpContext.User.Claims.ToArray()[1].Value;

        [HttpPut, Authorize]
        public IResult Put([FromBody] UserProfileData profileData)
        {
            try
            {
                //profileData.
            }
            catch (BaseException exp)
            {
            }

            return Results.Ok();
        }
    }
}
