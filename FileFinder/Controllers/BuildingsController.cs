using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileFinder.Data;
using FileFinder.Models;
using Microsoft.AspNetCore.Authorization;
using FileFinder.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FileFinder.Controllers
{
    //[Authorize]
    public class BuildingsController : Controller
    {
        private readonly FileFinderContext _context;

        public BuildingsController(FileFinderContext context)
        {
            _context = context;
        }

        //TODO: Combine Rooms and Buildings Controller/Views into "Location"

        // GET: Buildings
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

            return View(await _context.Buildings.OrderBy(b => b.Name).Include(b => b.Rooms).ToListAsync());
        }

        // GET: Buildings/Details/5
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

            var building = await _context.Buildings
                .SingleOrDefaultAsync(m => m.ID == id);

            building.Rooms = _context.Rooms.Where(r => r.BuildingID == building.ID).OrderBy(r => r.Name).ToList();

            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
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

            CreateBuildingViewModel createBuildingVM = new CreateBuildingViewModel();

            return View(createBuildingVM);
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBuildingViewModel createBuildingVM)
        {
            if (ModelState.IsValid)
            {
                Building newBuilding = new Building
                {
                    Name = createBuildingVM.Name,
                    Address = createBuildingVM.Address,
                    PhoneNumber = createBuildingVM.PhoneNumber
                };

                _context.Add(newBuilding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            return View(createBuildingVM);
        }

        // GET: Buildings/Edit/5
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

            var building = await _context.Buildings.SingleOrDefaultAsync(m => m.ID == id);
            if (building == null)
            {
                return NotFound();
            }

            EditBuildingViewModel editBuildingVM = new EditBuildingViewModel
            {
                Name = building.Name,
                Address = building.Address,
                PhoneNumber = building.PhoneNumber,
            };

            return View(editBuildingVM);
        }

        // POST: Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBuildingViewModel editBuildingVM)
        {
            if (ModelState.IsValid)
            {
                Building BuildingtoEdit = _context.Buildings.Single(cm => cm.ID == editBuildingVM.ID);

                BuildingtoEdit.Name = editBuildingVM.Name;
                BuildingtoEdit.Address = editBuildingVM.Address;
                BuildingtoEdit.PhoneNumber = editBuildingVM.PhoneNumber;

                _context.Update(BuildingtoEdit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(editBuildingVM);
        }

        // GET: Buildings/Delete/5
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

            var building = await _context.Buildings
                .SingleOrDefaultAsync(m => m.ID == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.SingleOrDefaultAsync(m => m.ID == id);
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.ID == id);
        }
    }
}
