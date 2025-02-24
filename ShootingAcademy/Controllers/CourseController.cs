﻿using Microsoft.AspNetCore.Authorization;
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
            try
            {
                IEnumerable<Course> Courses = await _context.Courses.AsNoTracking().ToListAsync();

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


        [HttpGet("user"), Authorize]
        public async Task<IResult> GetUserCourses()
        {
            try
            {
                Guid userGuid = AutorizeData.FromContext(HttpContext).UserGuid;

                User user = await _context.Users
                    .Include(i => i.Courses)
                    .FirstAsync(i => i.Id == userGuid);

                return Results.Json(user.Courses.Select(courseTicket =>
                {
                    Course course = _context.Courses
                        .First(i => i.Id == courseTicket.CourseId);

                    // 0_o
                    Random rand = new Random();

                    return new MyCourseBannerType()
                    {
                        completed_percent = rand.Next(0, 100),
                        duration = course.Duration,
                        finished_at = courseTicket.FinishedAt.ToString(),
                        started_at = courseTicket.StartedAt.ToString(),
                        id = course.Id.ToString(),
                        is_closed = courseTicket.IsClosed,
                        level = course.Level,
                        title = course.Title
                    };
                }).ToArray());
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
