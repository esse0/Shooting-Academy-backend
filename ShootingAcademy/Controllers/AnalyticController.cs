using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models.Exceptions;
using ShootingAcademy.Models;
using ShootingAcademy.Services;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.Controllers.Analytic;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AnalyticController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private Dictionary<string, float?> GetScoreByMonth(List<CompetitionMember> competitions)
        {
            Dictionary<string, float?> scoreByMonth = new Dictionary<string, float?>();

            foreach (CompetitionMember item in competitions)
            {
                DateTime date = item.Competition.DateTime;
                string month = date.ToString("MMMM");

                if (scoreByMonth.ContainsKey(month))
                {
                    scoreByMonth.Add(month, item.Result);
                }
                else scoreByMonth[month] += item.Result;
            }

            return scoreByMonth;
        }


        [HttpGet, Authorize]
        public async Task<IResult> GetAnalitics()
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await dbContext.Users.Include(usr => usr.Courses)
                    .Include(usr => usr.Competitions).FirstAsync(usr => usr.Id == userGuid);

                var lessonsCount = user.Courses.Select(x => x.CompletedLessons).ToList().Count;

                var courseIds = user.Courses.Where(x => x.IsClosed).Select(x => x.CourseId);

                var coursesByCategory = await dbContext.Courses.
                    Where(x => courseIds.Contains(x.Id)).Select(x => x.Category).ToListAsync();

                var competitionIds = user.Competitions.Select(x => x.Id);

                var competitions = await dbContext.Competitions.
                    Where(x => competitionIds.Contains(x.Id) && x.Status == Competition.ActiveStatus.Ended).ToListAsync();

                var scoreByMonth = GetScoreByMonth(user.Competitions);


                return Results.Json(new AnalyticResponse()
                {
                    lessonsCount = lessonsCount,
                    coursesByCategory = coursesByCategory,
                    competitions = competitions,
                    scoreByMonth = scoreByMonth,
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
    }
}
