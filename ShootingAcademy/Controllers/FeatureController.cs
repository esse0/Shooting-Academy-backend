using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.Features;
using ShootingAcademy.Models.DB;

namespace ShootingAcademy.Controllers
{
    public class FeatureController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public FeatureController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] form)
        {
            try
            {
                IEnumerable<Feature> Users = await _context.Features.AsNoTracking().ToListAsync();

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
        public async Task<IActionResult> Post([FromBody] form)
        {
            try
            {

                _context.Features.Add();

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
        public async Task<IActionResult> Put([FromBody] form)
        {
            try
            {

                _context.Features.Update();

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
        public async Task<IActionResult> Delete(id)
        {
            try
            {
                _context.Features.Remove();

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
