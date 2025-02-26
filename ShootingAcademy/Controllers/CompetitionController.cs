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
                    maxMebmerCount = competion.MaxMembersCount,
                    memberCount = competion.Members?.Count ?? 0,
                    date = competion.DateTime.ToShortDateString(),
                    time = competion.DateTime.ToShortTimeString(),
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

        [HttpPut("createCompetition"), Authorize]
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

                if (!DateTime.TryParse($"{competition.date} {competition.time}", out DateTime competitionDateTime))
                    throw new BaseException("Invalid date or time format.", 400);

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

                _context.Competions.Add(newCompetition);

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
    }
}
