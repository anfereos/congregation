using Congregation.Web.Data.Entities;
using Congregation.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Congregation.Web.Controllers
{
    [Authorize(Roles = "Admin")]//Si esta aquí este rol tiene acceso a todos los metodos de este controlador
    public class ProfessionsController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public ProfessionsController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Professions.ToListAsync());
        }

        // GET: Professions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Profession profession = await _context.Professions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profession == null)
            {
                return NotFound();
            }

            return View(profession);
        }

        // GET: Professions/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Profession profession)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(profession);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(profession);
        }

        // GET: Professions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Profession profession = await _context.Professions.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }
            return View(profession);
        }

        // POST: Professions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profession profession)
        {
            if (id != profession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profession);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(profession);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Profession profession = await _context.Professions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profession == null)
            {
                return NotFound();
            }

            try
            {
                _context.Professions.Remove(profession);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("The profession was deleted.");
            }
            catch
            {
                _flashMessage.Danger("The profession can't be deleted because it has related records.");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProfessionExists(int id)
        {
            return _context.Professions.Any(e => e.Id == id);
        }
    }
}
