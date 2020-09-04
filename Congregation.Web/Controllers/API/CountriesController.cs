using Congregation.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Congregation.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            return Ok(_context.Countries
                .Include(c => c.Districts)
                .ThenInclude(d => d.Churches));
        }
    }

}
