using Microsoft.AspNetCore.Mvc;
using ShootingAcademy.Models.Controllers.Features;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models;
using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.Exceptions;

namespace ShootingAcademy.Controllers
{
    public class GroupMemberController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public GroupMemberController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IResult> Get([FromQuery] object form)
        {
            try
            {
                IEnumerable<GroupMember> GroupMembers = await _context.GroupMembers.AsNoTracking().ToListAsync();

                return Results.Ok(new FeatureResponse()
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

                //_context.GroupMembers.Add();

                return Results.Ok(new FeatureResponse());
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

                //_context.GroupMembers.Update();

                return Results.Ok(new FeatureResponse());
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
                //_context.GroupMembers.Remove();

                return Results.Ok(new FeatureResponse());

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
