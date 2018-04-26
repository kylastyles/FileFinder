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
    public class RoomsController : Controller
    {
        private readonly FileFinderContext _context;

        public RoomsController(FileFinderContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var fileFinderContext = _context.Rooms.Include(r => r.Building).OrderBy(r => r.Name).ToListAsync();

            return View(await fileFinderContext);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Building)
                .SingleOrDefaultAsync(m => m.ID == id);

            // TODO: Add files to rooms
            //room.Files = _context.Files.Where(f => f.RoomID == room.ID).ToList();
            // Step 2, query File table to get relevent data

            ViewData["files"] = _context.Files.Where(f => f.RoomID == room.ID).ToList();

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            //ViewData["BuildingID"] = new SelectList(_context.Buildings, "ID", "ID");

            CreateRoomViewModel createRoomVM = new CreateRoomViewModel();
            createRoomVM.Buildings = _context.Buildings.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.Name }).ToList();

            return View(createRoomVM);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,BuildingID")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingID"] = new SelectList(_context.Buildings, "ID", "ID", room.BuildingID);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.SingleOrDefaultAsync(m => m.ID == id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["BuildingID"] = new SelectList(_context.Buildings, "ID", "ID", room.BuildingID);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,BuildingID")] Room room)
        {
            if (id != room.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.ID))
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
            ViewData["BuildingID"] = new SelectList(_context.Buildings, "ID", "ID", room.BuildingID);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Building)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(m => m.ID == id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.ID == id);
        }
    }
}
