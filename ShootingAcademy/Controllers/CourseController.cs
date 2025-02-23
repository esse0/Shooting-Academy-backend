using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.Course;

namespace ShootingAcademy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CourseController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("/GetMyCourseBannerType"), Authorize]
        public IResult GetMyCourseBannerType()
        {
            try
            {
                MyCourseBannerType myCourseBannerType = new MyCourseBannerType();

                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        }
    }
}
