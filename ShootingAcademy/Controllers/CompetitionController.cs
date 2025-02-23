using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.Course;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace ShootingAcademy.Controllers
{
    public class CompetitionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public CompetitionController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] object form)
        {
            try
            {
                IEnumerable<Competion> Users = await _context.Competions.AsNoTracking().ToListAsync();

                return Ok(new CompetitionResponse()
                {

                });

            }
            catch (Exception error)
            {
                return BadRequest(new CompetitionResponse()
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

                //_context.Competions.Add();

                return Ok(new CompetitionResponse());
            }
            catch (Exception error)
            {
                return BadRequest(new CompetitionResponse()
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

                //_context.Competions.Update();

                return Ok(new CompetitionResponse());
            }
            catch (Exception error)
            {
                return BadRequest(new CompetitionResponse()
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
                //_context.Competions.Remove();

                return Ok(new CompetitionResponse());

            }
            catch (Exception error)
            {
                return BadRequest(new CompetitionResponse()
                {
                    error = error.Message,
                });
            }
        }
    }
}
