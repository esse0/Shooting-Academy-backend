using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.Features;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using ShootingAcademy.Services;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Models.Controllers.AthleteGroup;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AthleteGroupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AthleteGroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("user"), Authorize]
        public async Task<IResult> GetUserGroups()
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await _context.Users
                                .Include(usr => usr.AthleteGroups)
                                .FirstAsync(usr => usr.Id == userGuid);

                var userGroups = await _context.AthleteGroups
                                         .Where(group => user.AthleteGroups.Any(gm => gm.AthleteGroupId == group.Id))
                                         .Include(group => group.Coatch)
                                         .Include(group => group.Athletes)
                                         .ToArrayAsync();

                return Results.Json(userGroups.Select(group =>
                {
                    var users = _context.Users
                                .Where(usr => group.Athletes.Any(ath => ath.Id == usr.Id))
                                .ToArray();

                    return new AthleteGroupModel()
                    {
                        OrganizationName = group.OrganizationName,
                        id = group.Id.ToString(),
                        coach = FullUserModel.FromEntity(group.Coatch),
                        members = users.Select(user => FullUserModel.FromEntity(user)).ToList(),
                    };
                }));
            }
            catch (BaseException apperr)
            {
                return Results.Json(apperr.GetModel(), statusCode: apperr.Code);
            }
        }
    }
}

