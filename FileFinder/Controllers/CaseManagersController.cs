using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileFinder.Data;
using FileFinder.Models;
using FileFinder.ViewModels;

namespace FileFinder.Controllers
{
    public class CaseManagersController : Controller
    {
        private readonly FileFinderContext _context;

        public CaseManagersController(FileFinderContext context)
        {
            _context = context;
        }

        // GET: CaseManagers
        public async Task<IActionResult> Index()
        {
            var fileFinderContext = _context.CaseManagers.Include(c => c.Program);
            return View(await fileFinderContext.OrderBy(cm => cm.FullName()).ToListAsync());
        }

        // GET: CaseManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseManager = await _context.CaseManagers
                .Include(c => c.Program)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (caseManager == null)
            {
                return NotFound();
            }

            return View(caseManager);
        }

        // GET: CaseManagers/Create
        public IActionResult Create()
        {
            CreateCaseManagerViewModel createCMVM = new CreateCaseManagerViewModel();

            createCMVM.Programs = _context.Programs.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name }).ToList();

            return View(createCMVM);
        }

        // POST: CaseManagers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Email,PhoneNumber,ProgramID,LastName,FirstName")] CaseManager caseManager)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(caseManager);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Program"] = new SelectList(_context.Programs, "ID", "Name", caseManager.Program);
        //    return View(caseManager);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCaseManagerViewModel createCMVM)
        {
            if(ModelState.IsValid)
            {

                CaseManager cm = new CaseManager()
                {
                    LastName = createCMVM.LastName,
                    FirstName = createCMVM.FirstName,
                    Email = createCMVM.Email,
                    PhoneNumber = createCMVM.PhoneNumber,
                    ProgramID = createCMVM.ProgramID
                };

                _context.Add(cm);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            createCMVM.Programs = _context.Programs.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name }).ToList();

            return View(createCMVM);
        }

        // GET: CaseManagers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var caseManager = await _context.CaseManagers.SingleOrDefaultAsync(m => m.ID == id);
        //    if (caseManager == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ProgramID"] = new SelectList(_context.Programs, "Name", "Name", caseManager.Program.Name);
        //    return View(caseManager);
        //}

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CaseManager CMtoEdit = _context.CaseManagers.Single(cm => cm.ID == id);

            EditCaseManagerViewModel EditCMVM = new EditCaseManagerViewModel
            {
                ID = CMtoEdit.ID,
                LastName = CMtoEdit.LastName,
                FirstName = CMtoEdit.FirstName,
                Email = CMtoEdit.Email,
                PhoneNumber = CMtoEdit.PhoneNumber,
                ProgramID = CMtoEdit.ProgramID
            };

            EditCMVM.Programs = _context.Programs.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name }).ToList();

            return View(EditCMVM);
        }

        // POST: CaseManagers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Email,PhoneNumber,ProgramID,LastName,FirstName")] CaseManager caseManager)
        {
            if (id != caseManager.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseManagerExists(caseManager.ID))
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
            ViewData["Program"] = new SelectList(_context.Programs, "Name", "Name", caseManager.Program.Name);
            return View(caseManager);
        }

        // GET: CaseManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseManager = await _context.CaseManagers
                .Include(c => c.Program)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (caseManager == null)
            {
                return NotFound();
            }

            return View(caseManager);
        }

        // POST: CaseManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseManager = await _context.CaseManagers.SingleOrDefaultAsync(m => m.ID == id);
            _context.CaseManagers.Remove(caseManager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseManagerExists(int id)
        {
            return _context.CaseManagers.Any(e => e.ID == id);
        }
    }
}
