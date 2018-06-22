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
    public class ConsumersController : Controller
    {
        private readonly FileFinderContext _context;

        public ConsumersController(FileFinderContext context)
        {
            _context = context;
        }

        // GET: Consumer
        public async Task<IActionResult> Index()
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            return View(await _context.Consumers.OrderBy(c=> c.FullName()).ToListAsync());
        }

        // GET: Consumer/Details/5 
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

            var consumer = await _context.Consumers
                .SingleOrDefaultAsync(m => m.ID == id);
            if (consumer == null)
            {
                return NotFound();
            }

            // Add files to consumer
            consumer.Files = _context.Files.Where(f => f.ConsumerID == consumer.ID)
                                           .Include(f => f.CaseManager)
                                           .Include(f => f.Room)
                                           .ToList();

            return View(consumer);
        }

        // GET: Consumer/Create
        public IActionResult Create()
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            CreateConsumerViewModel createConsumerVM = new CreateConsumerViewModel(); 

            return View(createConsumerVM);
        }

        // POST: Consumer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateConsumerViewModel createConsumerVM)
        {
            if (ModelState.IsValid)
            {
                Consumer newConsumer = new Consumer
                {
                    LastName = createConsumerVM.LastName,
                    FirstName = createConsumerVM.FirstName,
                    DOB = createConsumerVM.DOB
                };

                _context.Add(newConsumer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(createConsumerVM);
        }

        // GET: Consumer/Edit/5
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

            // Find consumer
            Consumer consumerToEdit = await _context.Consumers.SingleOrDefaultAsync(m => m.ID == id);
            if (consumerToEdit == null)
            {
                return NotFound();
            }

            EditConsumerViewModel editConsumerVM = new EditConsumerViewModel
            {
                ID = consumerToEdit.ID,
                LastName = consumerToEdit.LastName,
                FirstName = consumerToEdit.FirstName,
                DOB = consumerToEdit.DOB,
                Active = consumerToEdit.Active,
                EndDate = consumerToEdit.EndDate
            };

            return View(editConsumerVM);
        }

        // POST: Consumer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditConsumerViewModel editConsumerVM)
        {
            if(ModelState.IsValid)
            {
                //Get consumer
                Consumer consumerToEdit = _context.Consumers.Single(c => c.ID == editConsumerVM.ID);
                // Get associated files
                consumerToEdit.Files = _context.Files.Where(f => f.ConsumerID == consumerToEdit.ID).ToList();

                consumerToEdit.LastName = editConsumerVM.LastName;
                consumerToEdit.FirstName = editConsumerVM.FirstName;
                consumerToEdit.DOB = editConsumerVM.DOB;

                // ONLY when a consumer's active state is changed (so it won't affect file statuses otherwise):
                if (editConsumerVM.Active != consumerToEdit.Active)
                {
                    // If consumer becomes inactive, add EndDate 
                    if (editConsumerVM.Active == false)
                    {
                        consumerToEdit.Active = false;
                        consumerToEdit.EndDate = editConsumerVM.EndDate;
                        if(consumerToEdit.Files.Count != 0)
                        {
                            // Change status of files to "Inactive" and set file ShredDate
                            foreach (File file in consumerToEdit.Files)
                            {
                                file.SetShredDate(editConsumerVM);
                                _context.Update(file);
                            }
                        }
                    }

                    // If inactive consumer becomes active, wipe EndDate and change status of files to "OK"
                    if (editConsumerVM.Active == true)
                    {
                        consumerToEdit.Active = true;
                        consumerToEdit.EndDate = null;
                        if (consumerToEdit.Files != null)
                        {
                            foreach (File file in consumerToEdit.Files)
                            {
                                file.Status = Status.OK;
                                file.SetShredDate(null);
                                _context.Update(file);
                            }
                        }
                    }
                }

                // If the active state remains unchanged, but an inactive consumer's EndDate is changed:
                if (editConsumerVM.EndDate != consumerToEdit.EndDate)
                {
                    // NOT that we'll allow it to be wiped...
                    if (editConsumerVM.EndDate == null && editConsumerVM.Active == false)
                    {
                        consumerToEdit.EndDate = DateTime.Now;
                    } else // Otherwise, set the change to the consumer
                    {
                        consumerToEdit.EndDate = editConsumerVM.EndDate;
                    }
                    // Update their files' ShredDate
                    if(consumerToEdit.Files != null)
                    {
                        foreach (File file in consumerToEdit.Files)
                        {
                            file.SetShredDate(editConsumerVM);
                            _context.Update(file);
                        }
                    }
                }

                _context.Update(consumerToEdit);
                await _context.SaveChangesAsync();

                return Redirect("/Consumers/Index");
            };

            return View(editConsumerVM);
        }

        // GET: Consumer/Delete/5
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

            var consumer = await _context.Consumers
                .SingleOrDefaultAsync(m => m.ID == id);
            if (consumer == null)
            {
                return NotFound();
            }

            return View(consumer);
        }

        // POST: Consumer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consumer = await _context.Consumers.SingleOrDefaultAsync(m => m.ID == id);
            _context.Consumers.Remove(consumer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsumerExists(int id)
        {
            return _context.Consumers.Any(e => e.ID == id);
        }
    }
}
