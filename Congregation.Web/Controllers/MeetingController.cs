using Congregation.Web.Data;
using Congregation.Web.Data.Entities;
using Congregation.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Congregation.Web.Controllers
{
    public class MeetingController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MeetingController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);


            return View(await _context.Meetings
                .Include(d => d.Assistances)
                .Include(m => m.Church)
                .Where(m => m.Church.Id == user.Church.Id)
                .ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Meeting meeting = await _context.Meetings
                    .Include(d => d.Assistances)
                    .ThenInclude(u => u.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

    }
}