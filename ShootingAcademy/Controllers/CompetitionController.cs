using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.Course;
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
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public CompetitionController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("user"), Authorize]
        public async Task<IResult> GetUserCompetions([FromQuery] bool history = false)
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await _context.Users
                    .Include(i => i.Competions)
                    .FirstAsync(i => i.Id == userGuid);

                List<CompetionTypes> competionTypes = [];

                foreach (var competionTicket in user.Competions)
                {
                    Competion competion = _context.Competions
                        .Include(com => com.Organization)
                        .Include(com => com.Members)
                        .First(i => i.Id == competionTicket.CompetionId);

                    if (!history && competion.Status == Competion.ActiveStatus.Ended)
                        continue;

                    competionTypes.Add(new CompetionTypes()
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
                    });
                }

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

        [HttpGet]
        public async Task<IResult> Get([FromQuery] object form)
        {
            try
            {
                IEnumerable<Competion> Competions = await _context.Competions.AsNoTracking().ToListAsync();

                return Results.Ok(new CompetitionResponse()
                {

                });

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


        [HttpPost]
        public async Task<IResult> Post([FromBody] object form)
        {
            try
            {

                //_context.Competions.Add();

                return Results.Ok(new CompetitionResponse());
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

        [HttpPut]
        public async Task<IResult> Put([FromBody] object form)
        {
            try
            {

                //_context.Competions.Update();

                return Results.Ok(new CompetitionResponse());
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

        [HttpDelete]
        public async Task<IResult> Delete(Guid id)
        {
            try
            {
                //_context.Competions.Remove();

                return Results.Ok(new CompetitionResponse());

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
