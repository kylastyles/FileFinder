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
    public class FilesController : Controller
    {
        private readonly FileFinderContext _context;

        public FilesController(FileFinderContext context)
        {
            _context = context;
        }

        // GET: Files
        public async Task<IActionResult> Index()
        {
            var fileFinderContext = _context.Files.Include(f => f.CaseManager).Include(f => f.Consumer).Include(f => f.Room);
            return View(await fileFinderContext.OrderBy(f => f.Consumer.LastName).ThenBy(f => f.Consumer.FirstName).ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .Include(f => f.CaseManager)
                .Include(f => f.Consumer)
                .Include(f => f.Room)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            CreateFileViewModel createFileVM = new CreateFileViewModel
            {
                CaseManagers = _context.CaseManagers.Select(cm => new SelectListItem() { Value = cm.ID.ToString(), Text = cm.FullName() }).ToList(),
                Consumers = _context.Consumers.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FullName() }).ToList(),
                Rooms = _context.Rooms.Select(r => new SelectListItem() { Value = r.ID.ToString(), Text = r.Name }).ToList()
            };

            //ViewData["CaseManagerID"] = _context.CaseManagers.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.FullName() }).ToList();
            //ViewData["ConsumerID"] = _context.Consumers.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.FullName() }).ToList();
            //ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name");

            return View(createFileVM);
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFileViewModel createFilevm)
        {
            if (ModelState.IsValid)
            { 
                File newFile = new File
                {
                    ConsumerID = createFilevm.ConsumerID,
                    CaseManagerID = createFilevm.CaseManagerID,
                    RoomID = createFilevm.RoomID,
                    Quantity = createFilevm.Quantity
                };

                if(_context.FileExists(newFile))
                {
                    // TODO: Make a modal pop-up that confirms user wants to add quantity to existing file
                    File oldFile = _context.Files.First(f => f.CaseManagerID == newFile.CaseManagerID && f.ConsumerID == newFile.ConsumerID && f.RoomID == newFile.RoomID);
                    oldFile.Quantity += newFile.Quantity;
                    _context.Update(oldFile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                };

                _context.Add(newFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //Is this stuff needed? Will Model ever not be valid? -K 4/20/18 8:23pm
            //ViewData["CaseManagerID"] = new SelectList(_context.CaseManagers, "ID", "FirstName", file.CaseManagerID);
            //ViewData["ConsumerID"] = new SelectList(_context.Consumers, "ID", "FirstName", file.ConsumerID);
            //ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "ID", file.RoomID);
            return View(createFilevm);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files.SingleOrDefaultAsync(m => m.ID == id);
            if (file == null)
            {
                return NotFound();
            }

            EditFileViewModel editFilevm = new EditFileViewModel
            {
                // Fill Select Lists
                CaseManagers = _context.CaseManagers.Select(cm => new SelectListItem() { Value = cm.ID.ToString(), Text = cm.FullName() }).ToList(),
                Consumers = _context.Consumers.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FullName() }).ToList(),
                Rooms = _context.Rooms.Select(r => new SelectListItem() { Value = r.ID.ToString(), Text = r.Name }).ToList(),

                // Set known fields
                ID = file.ID,
                ConsumerID = file.ConsumerID,
                CaseManagerID = file.CaseManagerID,
                RoomID = file.RoomID,
                Quantity = file.Quantity,
                Status = file.Status,
                ShredDate = file.ShredDate
            };

            //ViewData["CaseManagerID"] = new SelectList(_context.CaseManagers, "ID", "FirstName", file.CaseManagerID);
            //ViewData["ConsumerID"] = new SelectList(_context.Consumers, "ID", "FirstName", file.ConsumerID);
            //ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "ID", file.RoomID);
            ViewData["StatusList"] = new SelectList(Enum.GetNames(typeof(Status)));
            return View(editFilevm);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Quantity,Status,ShredDate,ConsumerID,CaseManagerID,RoomID")] File file)
        {
            if (id != file.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.ID))
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
            ViewData["CaseManagerID"] = new SelectList(_context.CaseManagers, "ID", "FirstName", file.CaseManagerID);
            ViewData["ConsumerID"] = new SelectList(_context.Consumers, "ID", "FirstName", file.ConsumerID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "ID", file.RoomID);
            return View(file);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .Include(f => f.CaseManager)
                .Include(f => f.Consumer)
                .Include(f => f.Room)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.Files.SingleOrDefaultAsync(m => m.ID == id);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.Files.Any(e => e.ID == id);
        }
    }
}
