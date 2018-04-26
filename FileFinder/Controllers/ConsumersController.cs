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
            return View(await _context.Consumers.OrderBy(c=> c.FullName()).ToListAsync());
        }

        // GET: Consumer/Details/5 
        public async Task<IActionResult> Details(int? id)
        {
            // TODO: Add Files to Consumer Details
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

        // GET: Consumer/Create
        public IActionResult Create()
        {
            CreateConsumerViewModel createConsumerVM = new CreateConsumerViewModel(); 

            return View(createConsumerVM);
        }

        // POST: Consumer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,DOB,Active,LastName,FirstName")] Consumer consumer)
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
            if (id == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers.SingleOrDefaultAsync(m => m.ID == id);
            if (consumer == null)
            {
                return NotFound();
            }
            return View(consumer);
        }

        // POST: Consumer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DOB,Active,LastName,FirstName")] Consumer consumer)
        {
            if (id != consumer.ID)
            {
                return NotFound();
            }

            // TODO: Make EditConsumerViewModel
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consumer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsumerExists(consumer.ID))
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
            return View(consumer);
        }

        // GET: Consumer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
