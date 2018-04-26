using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileFinder.Models;
using FileFinder.ViewModels;
using FileFinder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly FileFinderContext _context;

        public HomeController(FileFinderContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // For Search
            HomeViewModel homeViewModel = new HomeViewModel();

            // For Daily Tasks
            homeViewModel.ActionFiles = _context.Files
                                        .Where(f => f.Status == Status.Damaged || f.Status == Status.Full)
                                        .Include(f => f.Consumer)
                                        .Include(f => f.CaseManager)
                                        .Include(f => f.Room);

            homeViewModel.InactiveFiles = _context.Files
                                        .Where(f => f.Status == Status.InactiveConsumer)
                                        .Include(f => f.Consumer)
                                        .Include(f => f.CaseManager)
                                        .Include(f => f.Room);

            return View(homeViewModel);
        }

        // <----------- PARTIAL VIEW EXPERIMENTS -------------->

        //public IActionResult Search()
        //{
        //    SearchViewModel searchViewModel = new SearchViewModel();

        //    return PartialView("_Search", searchViewModel);
        //}


        //// TODO: Make Daily Tasks List
        //public IActionResult DailyTasks()
        //{
        //    var query = from file in _context.Files.Include("Consumers")
        //                where file.Status != Status.OK || file.Status != null
        //                select file;

        //    return PartialView("_DailyTasks", query);
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
