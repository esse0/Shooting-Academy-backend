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
            IEnumerable<Competition> Competitions = await _context.Competitions
                .Where(i => i.Status != Competition.ActiveStatus.Ended)
                .Include(c => c.Organisation)
                .Include(c => c.Members)
                .AsNoTracking()
                .ToListAsync();

            return Results.Json(Competitions.Select(competition =>
            {
                return new CompetitionType
                {
                    status = Enum.GetName(typeof(Competition.ActiveStatus), competition.Status),
                    city = competition.City,
                    country = competition.Country,
                    maxMemberCount = competition.MaxMembersCount,
                    memberCount = competition.Members?.Count ?? 0,
                    date = competition.DateTime.ToLocalTime().ToShortDateString(),
                    time = competition.DateTime.ToLocalTime().ToShortTimeString(),
                    description = competition.Description,
                    exercise = competition.Exercise,
                    id = competition.Id.ToString(),
                    organiser = competition.Organisation != null
                        ? $"{competition.Organisation.FirstName} {competition.Organisation.SecoundName}"
                        : "Unknown",
                    title = competition.Title,
                    venue = competition.Venue,
                };
            }));
        }

        [HttpGet("fulldata")]
        public async Task<IResult> GetCompetitionById([FromQuery] string competitionId)
        {
            try
            {
                var competitionGuid = Guid.Parse(competitionId);

                var competition = await _context.Competitions
                    .Include(c => c.Organisation) // Включаем организацию
                    .Include(c => c.Members)      // Включаем участников
                    .ThenInclude(m => m.Athlete)  // Включаем информацию о спортсменах (если у вас есть такая модель)
                    .FirstOrDefaultAsync(c => c.Id == competitionGuid);

                if (competition == null)
                {
                    throw new BaseException("Competition not found", 404);
                }

                var competitionDetails = new CompetitionType
                {
                    id = competition.Id.ToString(),
                    title = competition.Title,
                    description = competition.Description,
                    date = competition.DateTime.ToLocalTime().ToShortDateString(),
                    time = competition.DateTime.ToLocalTime().ToShortTimeString(),
                    maxMemberCount = competition.MaxMembersCount,
                    memberCount = competition.Members.Count,
                    venue = competition.Venue,
                    country = competition.Country,
                    city = competition.City,
                    exercise = competition.Exercise,
                    status = Enum.GetName(typeof(Competition.ActiveStatus), competition.Status),
                    organiser = competition.Organisation != null
                        ? $"{competition.Organisation.FirstName} {competition.Organisation.SecoundName}"
                        : "Unknown",
                    members = competition.Members.Select(m => new Models.Controllers.Competition.CompetitionMember
                    {
                        id = m.Id.ToString(),
                        fullName = $"{m.Athlete.FirstName} {m.Athlete.SecoundName}",
                        age = m.Athlete.Age,
                        country = m.Athlete.Country,
                        grade = m.Athlete.Grade,
                        result = m.Result
                    }).ToList()
                };

                return Results.Json(competitionDetails);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: 400);
            }
        }


        [HttpGet("user"), Authorize]
        public async Task<IResult> GetUserCompetitions([FromQuery] bool history = false)
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await _context.Users
                    .Include(u => u.Competitions)
                    .ThenInclude(tc => tc.Competition)
                    .ThenInclude(c => c.Organisation)
                    .Include(u => u.Competitions)
                    .ThenInclude(tc => tc.Competition.Members)
                    .FirstAsync(u => u.Id == userGuid);

                var competitionTypes = user.Competitions
                    .Select(tc => tc.Competition)
                    .Where(competition => history ? competition.Status == Competition.ActiveStatus.Ended
                                                : competition.Status != Competition.ActiveStatus.Ended)
                    .Select(competition => new CompetitionType
                    {
                        status = Enum.GetName(typeof(Competition.ActiveStatus), competition.Status),
                        city = competition.City,
                        country = competition.Country,
                        maxMemberCount = competition.MaxMembersCount,
                        memberCount = competition.Members.Count,
                        date = competition.DateTime.ToLocalTime().ToShortDateString(),
                        time = competition.DateTime.ToLocalTime().ToShortTimeString(),
                        description = competition.Description,
                        exercise = competition.Exercise,
                        id = competition.Id.ToString(),
                        organiser = $"{competition.Organisation.FirstName} {competition.Organisation.SecoundName}",
                        title = competition.Title,
                        venue = competition.Venue
                    })
                    .ToList();

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


        [HttpGet("organisator"), Authorize(Roles = "organisator")]
        public async Task<IResult> GetOrganisatorCompetitions()
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                var competitions = await _context.Competitions
                    .Where(c => c.OrganisationId == userGuid)
                    .Include(c => c.Organisation)
                    .Include(c => c.Members)
                    .AsNoTracking()
                    .ToListAsync();

                var competitionTypes = competitions.Select(c => new CompetitionType
                {
                    id = c.Id.ToString(),
                    title = c.Title,
                    description = c.Description,
                    date = c.DateTime.ToLocalTime().ToString("yyyy-MM-dd"),
                    time = c.DateTime.ToLocalTime().ToString("HH:mm"),
                    maxMemberCount = c.MaxMembersCount,
                    memberCount = c.Members.Count,
                    venue = c.Venue,
                    country = c.Country,
                    city = c.City,
                    exercise = c.Exercise,
                    status = Enum.GetName(typeof(Competition.ActiveStatus), c.Status),
                    organiser = $"{c.Organisation.FirstName} {c.Organisation.SecoundName}"
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
        public async Task<IResult> CreateCompetition([FromBody] CompetitionType competition)
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

                if(!DateTime.TryParse(competition.date, out DateTime competitionDate))
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

                var newCompetition = new Competition
                {
                    Id = Guid.NewGuid(),
                    Title = competition.title,
                    Description = competition.description,
                    DateTime = competitionDateTime,
                    MaxMembersCount = competition.maxMemberCount,
                    Venue = competition.venue,
                    City = competition.city,
                    Country = competition.country,
                    Status = Competition.ActiveStatus.Pending,
                    Exercise = competition.exercise,
                    OrganisationId = organiserGuid
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

                if (competition.OrganisationId != userGuid)
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
