using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.User;
using ShootingAcademy.Models.Exceptions;
using ShootingAcademy.Services;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AutorizeDataService autorizeData;

        public UserController(AutorizeDataService autorizeData)
        {
            this.autorizeData = autorizeData;
        }


        [HttpPut, Authorize]
        public IResult Put([FromBody] UserProfileData profileData)
        {
            try
            {
                var a = autorizeData.UserGuid;
            }
            catch (BaseException exp)
            {
            }

            return Results.Ok();
        }
    }
}
