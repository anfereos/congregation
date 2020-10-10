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

        //[HttpPost]
        //public async Task<IActionResult> PostAssistance([FromBody] AssistanceRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;//traigo el email del usuario logeado
        //    User user = await _userHelper.GetUserAsync(email);
        //    if (user == null)
        //    {
        //        return NotFound("Error001");
        //    }

        //    Meeting meeting = await _context.Meetings   //listo las reuniones y agrego las asistencias
        //        .Include(p => p.Assistances)
        //        .FirstOrDefaultAsync(p => p.Id == request.Meeting);
        //    if (meeting == null)
        //    {
        //        return NotFound("Error002");
        //    }

        //    if (meeting.Assistances == null)
        //    {
        //        meeting.Assistances = new List<Assistance>();
        //    }

        //    meeting.Assistances.Add(new Assistance
        //    {
        //        Date = request.Date,
        //        Meeting = meeting,
        //        IsPresent = request.IsPresent,
        //        User = user
        //    });

        //    _context.Meetings.Update(meeting);
        //    await _context.SaveChangesAsync();
        //    return Ok(meeting);
        //}

    }
}
