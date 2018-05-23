using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileFinder.Models;
using FileFinder.ViewModels;
using FileFinder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileFinder.Controllers
{
    public class AuditController : Controller
    {
        private FileFinderContext _context;

        public AuditController(FileFinderContext context)
        {
            _context = context;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("/Home/Login");
            }

            AuditViewModel auditViewModel = new AuditViewModel();
            return View(auditViewModel);
        }

        [HttpPost]
        public IActionResult AuditSort(AuditViewModel auditViewModel)
        {
            if (auditViewModel == null)
            {
                return Redirect("Index");
            }

            Audit audit = new Audit
            {
                FiscalYear = auditViewModel.FiscalYear,
            };

            IEnumerable<File> allFiles = _context.Files.Where(f => f.Status != Status.Inactive)
                .Include(f => f.Consumer).Include(f => f.Room).OrderBy(f => f.Room.Name)
                .ThenBy(f => f.Consumer.LastName).ThenBy(f => f.Consumer.FirstName);

            List<List<File>> lists = audit.SortFiles(allFiles);

            audit.FirstQuarter = lists[0];
            audit.SecondQuarter = lists[1];
            audit.ThirdQuarter = lists[2];
            audit.FourthQuarter = lists[3];

            return View(audit);
        }

    }

}
