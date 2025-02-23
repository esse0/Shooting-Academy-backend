using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.DB.ModelCourse;

namespace ShootingAcademy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICourseService _courseService;

        public CourseController(IConfiguration configuration, ICourseService courseService)
        {
            _configuration = configuration;
            _courseService = courseService;
        }


    }
}
