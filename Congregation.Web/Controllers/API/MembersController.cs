using Congregation.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Congregation.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : Controller
    {
        private readonly DataContext _context;
        public MembersController(DataContext context)
        {
            _context = context;
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
