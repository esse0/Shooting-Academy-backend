using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.User;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Models.Exceptions;
using ShootingAcademy.Services;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPut, Authorize]
        public async Task<IResult> Put([FromBody] UserProfileData profileData)
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                if (dbContext.Users.Where(usr => usr.Id != userGuid && usr.Email == profileData.Email).Any())
                    throw new BaseException("Данная почта занята!");

                User user = await dbContext.Users.FirstAsync(usr => usr.Id == userGuid);

                user.FirstName = profileData.Name;
                user.SecoundName = profileData.LastName;
                user.Address = profileData.Address;
                user.City = profileData.City;
                user.Grade = profileData.Grade;
                user.Age = profileData.Age;
                user.Email = profileData.Email;
                user.Age = profileData.Age;

                dbContext.Users.Update(user);

                await dbContext.SaveChangesAsync();

                return Results.Json(new FullUserModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    SecoundName = user.SecoundName,
                    PatronymicName = user.PatronymicName,
                    Email = user.Email,
                    Age = user.Age,
                    City = user.City,
                    Country = user.Country,
                    Address = user.Address,
                    Grade = user.Grade,
                    Role = user.Role,
                });
            }
            catch (BaseException exp)
            {
                return Results.Json(exp.GetModel(), statusCode: exp.Code);
            }
        }
    }
}
