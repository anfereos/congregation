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

    public class AssistanceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public AssistanceController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostAssistance([FromBody] AssistanceRequest request)
        //public async Task<IActionResult> PostAssistance(DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;//traigo el email del usuario logeado
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }


            Church church = await _context.Churches            // cargo la iglesia del usuario logueado
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == user.Church.Id);
            if (church == null)
            {
                return NotFound("Church not found.");
            }


            Meeting meeting = await _context.Meetings   //listo las reuniones y agrego las asistencias
                .Include(m => m.Church)
                .Include(m => m.Assistances)
                .ThenInclude(a => a.User)
                .ThenInclude(u => u.Profession)
                .FirstOrDefaultAsync(m => m.Date.Year == request.Date.Year &&
                                            m.Date.Month == request.Date.Month &&
                                            m.Date.Day == request.Date.Day &&
                                            m.Church.Id == user.Church.Id);

            if (meeting == null)
            {
                return NotFound("Error002");
            }

            if (meeting.Assistances == null)
            {
                meeting.Assistances = new List<Assistance>();
            }

            meeting.Assistances.Add(new Assistance
            {
                User = user,
                Meeting = meeting,
                IsPresent = request.IsPresent
            });

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            return Ok(meeting);
        }
    }
}
