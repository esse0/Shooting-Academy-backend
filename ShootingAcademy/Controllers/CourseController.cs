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
        public async Task<IResult> Get()
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

        [HttpGet("fulldata")]
        public async Task<IResult> GetCourseFullData([FromQuery] string id)
        {
            try
            {
                Guid cguid = Guid.Parse(id);

                Course course = await _context.Courses
                    .Include(c => c.Instructor)
                    .Include(c => c.Modules)
                    .ThenInclude(m => m.Lessons)
                    .Include(c => c.Faqs)
                    .Include(c => c.Features)
                    .FirstAsync(i => i.Id == cguid);

                return Results.Json(new CourseModel()
                {
                    Id = course.Id.ToString(),
                    category = course.Category,
                    description = course.Description,
                    duration = course.Duration,
                    title = course.Title,
                    is_closed = course.IsClosed,
                    level = course.Level,
                    peapeopleRateCount = course.PeopleRateCount,
                    rate = course.Rate,

                    instructor = FullUserModel.FromEntity(course.Instructor),

                    modules = course.Modules.Select(module =>
                    {
                        return new ModuleModel()
                        {
                            id = module.Id.ToString(),
                            title = module.Title,
                            lessons = module.Lessons.Select(lesson =>
                            {
                                return new LessonModel()
                                {
                                    id = lesson.Id.ToString(),
                                    description = lesson.Description,
                                    title = lesson.Title,
                                    videoLink = lesson.VideoLink,
                                };
                            }).ToList(),
                        };
                    }).ToList(),

                    faqs = course.Faqs.Select(faq =>
                    {
                        return new FraqModel()
                        {
                            id = faq.Id.ToString(),
                            answer = faq.Answer,
                            question = faq.Question,
                        };
                    }).ToList(),

                    features = course.Features.Select(feature =>
                    {
                        return new FeatureModel()
                        {
                            id = feature.Id.ToString(),
                            description = feature.Description,
                            title = feature.Title,
                        };
                    }).ToList()
                });
            }
            catch (BaseException exp)
            {
                return Results.Json(exp.GetModel(), statusCode: exp.Code);
            }
        }

        //[HttpPost("subscribe"), Authorize]
        //public async Task<IResult> SubscribeUser()
        //{

        //}
    }
}
