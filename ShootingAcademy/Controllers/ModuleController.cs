using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.Features;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace ShootingAcademy.Controllers
{
    public class ModuleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ModuleController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] object form)
        {
            try
            {
                IEnumerable<Module> Modules = await _context.Modules.AsNoTracking().ToListAsync();

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

                //_context.Modules.Add();

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

                //_context.Modules.Update();

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
                //_context.Modules.Remove();

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
