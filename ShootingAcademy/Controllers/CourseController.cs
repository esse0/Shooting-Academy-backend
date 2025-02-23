using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models;

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


        
    }
}
