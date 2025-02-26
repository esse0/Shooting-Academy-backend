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
                Random rand = new();

                var user = await _context.Users
                    .Include(u => u.Courses)
                    .ThenInclude(cm => cm.Course)
                    .FirstAsync(u => u.Id == userGuid);

                var courseBannerTypes = user.Courses
                .Where(cm => history ? cm.IsClosed : !cm.IsClosed)
                .Select(cm => new MyCourseBannerType
                {
                    completed_percent = rand.Next(0, 100),
                    duration = cm.Course.Duration,
                    finished_at = cm.FinishedAt.ToString(),
                    started_at = cm.StartedAt.ToString(),
                    id = cm.Course.Id.ToString(),
                    is_closed = cm.IsClosed,
                    level = cm.Course.Level,
                    title = cm.Course.Title
                })
                .ToList();

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
                if (!Guid.TryParse(id, out Guid cguid))
                    throw new BaseException("Invalid course ID format", 400);

                var courseData = await _context.Courses
                    .AsNoTracking()
                    .Where(c => c.Id == cguid)
                    .Select(course => new CourseModel
                    {
                        Id = course.Id.ToString(),
                        category = course.Category,
                        description = course.Description,
                        duration = course.Duration,
                        title = course.Title,
                        is_closed = course.IsClosed,
                        level = course.Level,
                        peopleRateCount = course.PeopleRateCount,
                        rate = course.Rate,
                        instructor = FullUserModel.FromEntity(course.Instructor),

                        modules = course.Modules
                            .Select(module => new ModuleModel
                            {
                                id = module.Id.ToString(),
                                title = module.Title,
                                lessons = module.Lessons
                                    .Select(lesson => new LessonModel
                                    {
                                        id = lesson.Id.ToString(),
                                        description = lesson.Description,
                                        title = lesson.Title,
                                        videoLink = lesson.VideoLink
                                    }).ToList()
                            }).ToList(),

                        faqs = course.Faqs
                            .Select(faq => new FraqModel
                            {
                                id = faq.Id.ToString(),
                                answer = faq.Answer,
                                question = faq.Question
                            }).ToList(),

                        features = course.Features
                            .Select(feature => new FeatureModel
                            {
                                id = feature.Id.ToString(),
                                description = feature.Description,
                                title = feature.Title
                            }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (courseData == null)
                    throw new BaseException("Course not found", 404);

                return Results.Json(courseData);
            }
            catch (BaseException exp)
            {
                return Results.Json(exp.GetModel(), statusCode: exp.Code);
            }
        }


        [HttpPost("subscribe"), Authorize]
        public async Task<IResult> SubscribeUser([FromQuery] string courseId)
        {
            try
            {
                if (!Guid.TryParse(courseId, out Guid cguid))
                    throw new BaseException("Invalid course ID format", 400);

                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                bool userAlreadySubscribed = await _context.CourseMembers
                    .AnyAsync(cm => cm.CourseId == cguid && cm.UserId == userGuid);

                if (userAlreadySubscribed)
                    throw new BaseException("The user is already on the course");

                Course? course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Id == cguid);

                if (course == null)
                    throw new BaseException($"Could not find course with ID {courseId}");

                _context.CourseMembers.Add(new CourseMember
                {
                    CourseId = cguid,
                    UserId = userGuid,
                    IsClosed = false,
                    CompletedLessons = new List<Guid>(),
                    StartedAt = DateTime.UtcNow,
                    FinishedAt = null
                });

                await _context.SaveChangesAsync();

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


        [HttpPost("leaveCourse"), Authorize]
        public async Task<IResult> UnsubscribeUser([FromQuery] string courseId)
        {
            try
            {
                if (!Guid.TryParse(courseId, out Guid cguid))
                    throw new BaseException("Invalid course ID format", 400);

                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                var courseMember = await _context.CourseMembers
                    .FirstOrDefaultAsync(cm => cm.CourseId == cguid && cm.UserId == userGuid);

                if (courseMember == null)
                    throw new BaseException("The user is not on the course", 404);

                courseMember.IsClosed = true;
                courseMember.FinishedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

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
