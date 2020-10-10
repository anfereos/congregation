using Congregation.Common.Request;
using Congregation.Web.Data;
using Congregation.Web.Data.Entities;
using Congregation.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Congregation.Web.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//requiere token
    [Route("api/[controller]")]
    public class MeetingController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MeetingController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("PostMeeting")]
        public async Task<IActionResult> PostMeeting([FromBody] MeetingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;//traigo el email del usuario logeado
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Church church = await _context.Churches
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == user.Church.Id);
            if(church == null)
            {
                return NotFound("Church no found");
            }


            Meeting meeting = await _context.Meetings
                .Include(m => m.Church)
                .FirstOrDefaultAsync(m => m.Id == request.Meeting.Id);

            meeting = new Meeting
            {
                Church = church,
                Date = request.Date,
                Assistances = new List<Assistance>()
            };

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
            return Ok(meeting);
        }


        //Meeting meeting = await _context.Meetings
        //    .Include(m => m.Church)
        //    .Include(m => m.Assistances)
        //    .FirstOrDefaultAsync(m => m.Id == request.Meeting.Id);    //Para buscar la Meeting q trae el request para usarlo en el metodo put



    }
}