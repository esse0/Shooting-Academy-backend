using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Services;
using ShootingAcademy.Models.Controllers.Competition;

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
            IEnumerable<Competion> Competions = await _context.Competitions
                .Where(i => i.Status != Competion.ActiveStatus.Ended)
                .Include(c => c.Organization)
                .Include(c => c.Members)
                .AsNoTracking()
                .ToListAsync();

            return Results.Json(Competions.Select(competion =>
            {
                return new CompetionType
                {
                    status = Enum.GetName(typeof(Competion.ActiveStatus), competion.Status),
                    city = competion.City,
                    country = competion.Country,
                    maxMemberCount = competion.MaxMembersCount,
                    memberCount = competion.Members?.Count ?? 0,
                    date = competion.DateTime.ToUniversalTime().ToString("yyyy-MM-dd"),
                    time = competion.DateTime.ToUniversalTime().ToString("HH:mm"),
                    description = competion.Description,
                    exercise = competion.Exercise,
                    id = competion.Id.ToString(),
                    organiser = competion.Organization != null
                        ? $"{competion.Organization.FirstName} {competion.Organization.SecoundName}"
                        : "Unknown",
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
                        maxMemberCount = competion.MaxMembersCount,
                        memberCount = competion.Members.Count,
                        date = competion.DateTime.ToUniversalTime().ToString("yyyy-MM-dd"),
                        time = competion.DateTime.ToUniversalTime().ToString("HH:mm"),
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


        [HttpGet("organisator"), Authorize(Roles = "organisator")]
        public async Task<IResult> GetOrganisatorCompetions()
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                var competitions = await _context.Competitions
                    .Where(c => c.OrganizationId == userGuid)
                    .Include(c => c.Organization)
                    .Include(c => c.Members)
                    .AsNoTracking()
                    .ToListAsync();

                var competitionTypes = competitions.Select(c => new CompetionType
                {
                    id = c.Id.ToString(),
                    title = c.Title,
                    description = c.Description,
                    date = c.DateTime.ToUniversalTime().ToString("yyyy-MM-dd"),
                    time = c.DateTime.ToUniversalTime().ToString("HH:mm"),
                    maxMemberCount = c.MaxMembersCount,
                    memberCount = c.Members.Count,
                    venue = c.Venue,
                    country = c.Country,
                    city = c.City,
                    exercise = c.Exercise,
                    status = Enum.GetName(typeof(Competion.ActiveStatus), c.Status),
                    organiser = $"{c.Organization.FirstName} {c.Organization.SecoundName}"
                }).ToList();

                return Results.Json(competitionTypes);
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

        [HttpPost("create"), Authorize(Roles = "organisator")]
        public async Task<IResult> CreateCompetition([FromBody] CompetionType competition)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(competition.title) ||
                    string.IsNullOrWhiteSpace(competition.description) ||
                    string.IsNullOrWhiteSpace(competition.date) ||
                    string.IsNullOrWhiteSpace(competition.time))
                {
                    throw new BaseException("Invalid input: required fields are missing.", 400);
                }

                if (competition.maxMemberCount <= 0)
                    throw new BaseException($"Max member count must be greater than 0.", 400);

                competition.date = competition.date.Remove(competition.date.IndexOf('T'));

                if (!DateTime.TryParse(competition.date, out DateTime competitionDate))
                    throw new BaseException("Invalid date or date format.", 400);

                if (!DateTime.TryParse(competition.time, out DateTime competitionTime))
                    throw new BaseException("Invalid time format.", 400);

                competitionDate = DateTime.SpecifyKind(competitionDate, DateTimeKind.Utc);
                competitionTime = DateTime.SpecifyKind(competitionTime, DateTimeKind.Utc);

                var competitionDateTime = competitionDate.Add(competitionTime.TimeOfDay);

                Guid organiserGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                var organiser = await _context.Users.FirstOrDefaultAsync(u => u.Id == organiserGuid);

                if (organiser == null)
                    throw new BaseException("Organizer not found.", 404);

                var newCompetition = new Competion
                {
                    Id = Guid.NewGuid(),
                    Title = competition.title,
                    Description = competition.description,
                    DateTime = competitionDateTime,
                    MaxMembersCount = competition.maxMemberCount,
                    Venue = competition.venue,
                    City = competition.city,
                    Country = competition.country,
                    Status = Competion.ActiveStatus.Pending,
                    Exercise = competition.exercise,
                    OrganizationId = organiserGuid
                };

                _context.Competitions.Add(newCompetition);

                await _context.SaveChangesAsync();

                return Results.StatusCode(201);
            }
            catch (BaseException apperr)
            {
                return Results.Json(apperr.GetModel(), statusCode: apperr.Code);
            }
            catch (Exception err)
            {
                return Results.Problem(err.Message, statusCode: 500);
            }
        }

        [HttpDelete("delete"), Authorize(Roles = "organisator")]
        public async Task<IResult> DeleteCompetition([FromQuery] string competitionId)
        {
            try
            {
                if (!Guid.TryParse(competitionId, out Guid competitionGuid))
                {
                    throw new BaseException("Invalid competition ID format.", code: 400);
                }

                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                var competition = await _context.Competitions
                .FirstOrDefaultAsync(c => c.Id == competitionGuid);

                if (competition == null)
                {
                    throw new BaseException("Competition not found.", code: 404);
                }

                if (competition.OrganizationId != userGuid)
                {
                    throw new BaseException("Unauthorized", code: 401);
                }

                _context.Competitions.Remove(competition);

                await _context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (BaseException apperr)
            {
                return Results.Json(apperr.GetModel(), statusCode: apperr.Code);
            }
            catch (Exception err)
            {
                return Results.Problem(err.Message, statusCode: 500);
            }
        }
    }
}
