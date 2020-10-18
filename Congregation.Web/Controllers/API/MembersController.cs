using Congregation.Web.Data;
using Congregation.Web.Data.Entities;
using Congregation.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Congregation.Web.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//requiere token
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MembersController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;//traigo el email del usuario logeado
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var members = await _context.Users
                .Where(u => u.UserType == Common.Enums.UserType.Member)
                .Include(c => c.Church).Where(c => c.Church.Id == user.Church.Id)
                .Include(p => p.Profession).ToListAsync();

            return Ok(members);
        }
    }
}
