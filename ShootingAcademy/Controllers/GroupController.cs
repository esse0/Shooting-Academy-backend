using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.Competition;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //public async Task<IResult> Get()
        //{

        //}

        //[HttpGet("organisator"), Authorize(Roles = "coach")]
        //public async Task<IResult> GetCoachGroups()
        //{

        //}

        //[HttpPost("addmember"), Authorize(Roles = "coach")]
        //public async Task<IResult> AddMember([FromQuery] string groupId, [FromQuery] string userId)
        //{

        //}

        //[HttpPost("kickmember"), Authorize(Roles = "coach")]
        //public async Task<IResult> KickMember([FromQuery] string groupId, [FromQuery] string userId)
        //{

        //}

        //[HttpPost("create"), Authorize(Roles = "coach")]
        //public async Task<IResult> CreateGroup([FromBody] CompetionType group)
        //{

        //}


        //[HttpDelete("delete"), Authorize(Roles = "coach")]
        //public async Task<IResult> DeleteGroup([FromQuery] string groupId)
        //{

        //}
    }
}
