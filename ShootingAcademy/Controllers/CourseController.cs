using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.Course;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models.DB.ModelUser;
using ShootingAcademy.Models.Exceptions;
using ShootingAcademy.Services;

namespace ShootingAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IResult> Get([FromQuery] object form)
        {
            IEnumerable<Course> Courses = await _context.Courses.ToListAsync();

            return Results.Json(Courses.Select(course =>
            {
                return new CourseModel()
                {
                    Id = course.Id.ToString(),
                    category = course.Category,
                    description = course.Description,
                    duration = course.Duration,
                    level = course.Level,
                    rate = course.Rate,
                    title = course.Title,
                };
            }));
        }

        [HttpGet("user"), Authorize]
        public async Task<IResult> GetUserCourses([FromQuery] bool history = false)
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await _context.Users
                    .Include(i => i.Courses)
                    .FirstAsync(i => i.Id == userGuid);

                List<MyCourseBannerType> courseBannerTypes = [];

                Random rand = new();

                foreach (var courseTicket in user.Courses)
                {
                    Course course = await _context.Courses
                                          .FirstAsync(i => i.Id == courseTicket.CourseId);

                    if (!history && courseTicket.IsClosed)
                        continue;

                    courseBannerTypes.Add(new MyCourseBannerType()
                    {
                        completed_percent = rand.Next(0, 100),
                        duration = course.Duration,
                        finished_at = courseTicket.FinishedAt.ToString(),
                        started_at = courseTicket.StartedAt.ToString(),
                        id = course.Id.ToString(),
                        is_closed = courseTicket.IsClosed,
                        level = course.Level,
                        title = course.Title
                    });
                }

                return Results.Json(courseBannerTypes);
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

                //_context.Courses.Add();

                return Results.Ok();
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

                //_context.Courses.Update();

                return Results.Ok();
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
                //_context.Courses.Remove(await _context.Courses.FirstAsync(i => i.Id == id));

                return Results.Ok();

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
