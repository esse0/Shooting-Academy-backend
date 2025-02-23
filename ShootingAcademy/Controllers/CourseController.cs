﻿using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models;
using ShootingAcademy.Models.Controllers.Course;
using ShootingAcademy.Models.DB;

namespace ShootingAcademy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public CourseController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult>  Get([FromQuery]  form)
        {
            try
            {
                IEnumerable<Course> Users = await _context.Courses.AsNoTracking().ToListAsync();

                return Ok(new CourseResponse()
                {
                    
                });

            }
            catch (Exception error)
            {
                return BadRequest(new CourseResponse()
                {
                    error = error.Message,
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]  form)
        {
            try
            {

                _context.Courses.Add();

                return Ok(new CourseResponse());
            }
            catch (Exception error)
            {
                return BadRequest(new CourseResponse()
                {
                    error = error.Message,
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]  form)
        {
            try
            {

                _context.Courses.Update();

                return Ok(new CourseResponse());
            }
            catch (Exception error)
            {
                return BadRequest(new CourseResponse()
                {
                    error = error.Message,
                });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete( id)
        {
            try
            {
                _context.Courses.Remove();

                return Ok(new CourseResponse());

            }
            catch (Exception error)
            {
                return BadRequest(new CourseResponse()
                {
                    error = error.Message,
                });
            }
        }


    }
}
