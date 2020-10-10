using Congregation.Web.Data;
using Congregation.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Congregation.Web.Controllers.API
{
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//requiere token
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
        public IActionResult GetMembers()
        {
            return Ok(_context.Users
                .Include(p => p.Church)
                .Include(p => p.Profession)
                .OrderBy(p => p.FullName));
        }

    }
}
