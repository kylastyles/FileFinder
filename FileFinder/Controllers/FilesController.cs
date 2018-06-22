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
using Microsoft.AspNetCore.Http;

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
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            var fileFinderContext = _context.Files.Include(f => f.CaseManager)
                                                  .Include(f => f.Consumer)
                                                  .Include(f => f.Room)
                                                  .OrderBy(f => f.Room.Name)
                                                  .ThenBy(f => f.CaseManager.LastName)
                                                  .ThenBy(f => f.Consumer.LastName)
                                                  .ThenBy(f => f.Consumer.FirstName);

            //Update files that need to be shredded
            foreach(File f in fileFinderContext)
            {
                if (f.ShredDate <= DateTime.Now)
                {
                    f.Status = Status.Shred;
                    _context.Update(f);
                }
            }
            _context.SaveChanges();

            return View(await fileFinderContext.ToListAsync());
        }

        // GET: Files/Details/5
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
        public IActionResult Create(int? id)
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            // Make ViewModel
            CreateFileViewModel createFileVM = new CreateFileViewModel
            {
                CaseManagers = _context.CaseManagers.Select(cm => new SelectListItem() { Value = cm.ID.ToString(), Text = cm.FullName() }).ToList(),
                Consumers = _context.Consumers.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FullName() }).ToList(),
                Rooms = _context.Rooms.Select(r => new SelectListItem() { Value = r.ID.ToString(), Text = r.Name }).ToList()
            };

            // Add Consumer name if id was passed in
            if (id != null)
            {
                createFileVM.ConsumerID = (int)id;
            }

            return View(createFileVM);
        }

        // POST: Files/Create
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

                //if file already exists, ask if user would like to update quantity
                if (_context.FileExists(newFile))
                {
                    File oldFile = _context.Files.First(f => f.CaseManagerID == newFile.CaseManagerID && f.ConsumerID == newFile.ConsumerID && f.RoomID == newFile.RoomID);

                    return Redirect(String.Format("AddConfirm?id={0}&userNum={1}", oldFile.ID, newFile.Quantity));
                };

                _context.Add(newFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(createFilevm);
        }

        public IActionResult AddConfirm(int? id, int? userNum)
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

            File oldFile = _context.Files.Single(f => f.ID == id);
            oldFile.Quantity += userNum ?? default(int);

            return View(oldFile);
        }

        [HttpPost]
        public IActionResult AddConfirm(File oldFile)
        {
            _context.Update(oldFile);
            _context.SaveChangesAsync();

            return Redirect(String.Format("Details?id={0}", oldFile.ID));
        }


        // GET: Files/Edit/5
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

            ViewData["StatusList"] = new SelectList(Enum.GetNames(typeof(Status)));
            return View(editFilevm);
        }

        // POST: Files/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditFileViewModel editFileViewModel)
        {
            if(ModelState.IsValid)
            {
                //Find file
                File fileToEdit = await _context.Files.SingleAsync(f => f.ID == editFileViewModel.ID);
                if (fileToEdit == null)
                {
                    return NotFound();
                }
                //Set changes
                fileToEdit.Quantity = editFileViewModel.Quantity;
                fileToEdit.ConsumerID = editFileViewModel.ConsumerID;
                fileToEdit.CaseManagerID = editFileViewModel.CaseManagerID;
                fileToEdit.RoomID = editFileViewModel.RoomID;
                fileToEdit.Status = editFileViewModel.Status;


                _context.Update(fileToEdit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(editFileViewModel);
        }

        // GET: Files/Delete/5
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
