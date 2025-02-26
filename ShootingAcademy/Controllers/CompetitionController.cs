using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Services;
using ShootingAcademy.Models.Controllers.Competition;
using ShootingAcademy.Models.Controllers.Course;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompetitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IResult> Get()
        {
            IEnumerable<Competion> Competions = await _context.Competions
                                                            .Where(i => i.Status != Competion.ActiveStatus.Ended)
                                                            .ToListAsync();

            return Results.Json(Competions.Select(competion =>
            {
                return new CompetionType
                {
                    status = Enum.GetName(competion.Status),
                    city = competion.City,
                    country = competion.Country,
                    maxMebmerCount = competion.MaxMembersCount,
                    memberCount = competion.Members.Count,
                    date = competion.DateTime.ToShortDateString(),
                    time = competion.DateTime.ToShortTimeString(),
                    description = competion.Description,
                    exercise = competion.Exercise,
                    id = competion.Id.ToString(),
                    organiser = $"{competion.Organization.FirstName} {competion.Organization.SecoundName}",
                    title = competion.Title,
                    venue = competion.Venue,
                };
            }));
        }

        [HttpGet("user"), Authorize]
        public async Task<IResult> GetUserCompetions([FromQuery] bool history = false)
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await _context.Users
                    .Include(u => u.Competions)
                    .ThenInclude(tc => tc.Competion)
                    .ThenInclude(c => c.Organization)
                    .Include(u => u.Competions)
                    .ThenInclude(tc => tc.Competion.Members)
                    .FirstAsync(u => u.Id == userGuid);

                var competionTypes = user.Competions
                    .Select(tc => tc.Competion)
                    .Where(competion => history ? competion.Status == Competion.ActiveStatus.Ended
                                                : competion.Status != Competion.ActiveStatus.Ended)
                    .Select(competion => new CompetionType
                    {
                        status = Enum.GetName(typeof(Competion.ActiveStatus), competion.Status),
                        city = competion.City,
                        country = competion.Country,
                        maxMebmerCount = competion.MaxMembersCount,
                        memberCount = competion.Members.Count,
                        date = competion.DateTime.ToShortDateString(),
                        time = competion.DateTime.ToShortTimeString(),
                        description = competion.Description,
                        exercise = competion.Exercise,
                        id = competion.Id.ToString(),
                        organiser = $"{competion.Organization.FirstName} {competion.Organization.SecoundName}",
                        title = competion.Title,
                        venue = competion.Venue
                    })
                    .ToList();

                return Results.Json(competionTypes);
            }
            catch (BaseException apperr)
            {
                return Results.Json(apperr.GetModel(), statusCode: apperr.Code);
            }

            catch (Exception err)
            {
                return Results.Problem(err.Message, statusCode: 400);
            }
        }
    }
}
