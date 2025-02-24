using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.Features;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace ShootingAcademy.Controllers
{
    public class CourseMemberController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public CourseMemberController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] object form)
        {
            try
            {
                IEnumerable<CourseMember> CourseMembers = await _context.CourseMembers.AsNoTracking().ToListAsync();

                return Ok(new FeatureResponse()
                {

                });

            }
            catch (Exception error)
            {
                return BadRequest(new FeatureResponse()
                {
                    error = error.Message,
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] object form)
        {
            try
            {

                //_context.CourseMembers.Add();

                return Ok(new FeatureResponse());
            }
            catch (Exception error)
            {
                return BadRequest(new FeatureResponse()
                {
                    error = error.Message,
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] object form)
        {
            try
            {

                //_context.CourseMembers.Update();

                return Ok(new FeatureResponse());
            }
            catch (Exception error)
            {
                return BadRequest(new FeatureResponse()
                {
                    error = error.Message,
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                //_context.CourseMembers.Remove();

                return Ok(new FeatureResponse());

            }
            catch (Exception error)
            {
                return BadRequest(new FeatureResponse()
                {
                    error = error.Message,
                });
            }
        }
    }
}
