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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FileFinder.Controllers
{
    //InvalidOperationException: No authenticationScheme was specified, and there was no DefaultChallengeScheme found.
    //Microsoft.AspNetCore.Authentication.AuthenticationService+<ChallengeAsync>d__11.MoveNext()

    //[Authorize]
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
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            //Change View based on Role:
            FileMember user = _context.FileMembers.Single(u => u.Email == HttpContext.Session.GetString("Username"));
            ViewBag.Role = user.Role;

            var fileFinderContext = _context.Rooms.Include(r => r.Building).OrderBy(r => r.Name).ToListAsync();

            return View(await fileFinderContext);
        }

        // GET: Rooms/Details/5
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

            //Change View based on Role:
            FileMember user = _context.FileMembers.Single(u => u.Email == HttpContext.Session.GetString("Username"));
            ViewBag.Role = user.Role;

            var room = await _context.Rooms
                .Include(r => r.Building)
                .SingleOrDefaultAsync(m => m.ID == id);

            // Add files to rooms
            room.Files = _context.Files.Where(f => f.RoomID == room.ID)
                        .Include(f => f.Consumer)
                        .Include(f => f.CaseManager)
                        .ToList();

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            //Deny non-users:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            //Deny non-admins:
            FileMember user = _context.FileMembers.Single(u => u.Email == HttpContext.Session.GetString("Username"));
            if (user.Role != Role.Admin)
            {
                return Redirect("/Buildings/Index");
            }

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
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            //Deny non-admins:
            FileMember user = _context.FileMembers.Single(u => u.Email == HttpContext.Session.GetString("Username"));
            if (user.Role != Role.Admin)
            {
                return Redirect("/Buildings/Index");
            }

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
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            //Deny non-admins:
            FileMember user = _context.FileMembers.Single(u => u.Email == HttpContext.Session.GetString("Username"));
            if (user.Role != Role.Admin)
            {
                return Redirect("/Buildings/Index");
            }

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
