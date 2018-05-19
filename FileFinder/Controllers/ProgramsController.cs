using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileFinder.Data;
using FileFinder.Models;
using Microsoft.AspNetCore.Http;

namespace FileFinder.Controllers
{
    public class ProgramsController : Controller
    {
        private readonly FileFinderContext _context;

        public ProgramsController(FileFinderContext context)
        {
            _context = context;
        }

        // GET: Programs
        public async Task<IActionResult> Index()
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            return View(await _context.Programs.OrderBy(p => p.Name).ToListAsync());
        }

        // GET: Programs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var Program = await _context.Programs
                .SingleOrDefaultAsync(m => m.ID == id);

            Program.CaseManagers = _context.CaseManagers.Where(cm => cm.ProgramID == Program.ID).OrderBy(cm => cm.FullName()).ToList();

            if (Program == null)
            {
                return NotFound();
            }

            return View(Program);
        }

        // GET: CPrograms/Create
        public IActionResult Create()
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            return View();
        }

        // POST: CPrograms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Models.Program program)
        {
            if (ModelState.IsValid)
            {
                _context.Add(program);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(program);
        }

        // GET: CPrograms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var program = await _context.Programs.SingleOrDefaultAsync(m => m.ID == id);
            if (program == null)
            {
                return NotFound();
            }
            return View(program);
        }

        // POST: Programs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Models.Program program)
        {
            if (id != program.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(program);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramExists(program.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(program);
        }

        // GET: Programs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var program = await _context.Programs
                .SingleOrDefaultAsync(m => m.ID == id);
            if (program == null)
            {
                return NotFound();
            }

            return View(program);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var program = await _context.Programs.SingleOrDefaultAsync(m => m.ID == id);
            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramExists(int id)
        {
            return _context.Programs.Any(e => e.ID == id);
        }
    }
}
