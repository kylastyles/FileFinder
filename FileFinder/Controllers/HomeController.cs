using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileFinder.Models;
using FileFinder.ViewModels;
using FileFinder.Data;

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
            SearchViewModel searchViewModel = new SearchViewModel();

            return View(searchViewModel);
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel searchViewModel)
        {

            if (searchViewModel.SelectedColumn.Equals(SearchFieldType.Consumer))
            {
                searchViewModel.ConsumerResults = _context.SearchConsumers(searchViewModel.UserInput);

            } else if (searchViewModel.SelectedColumn.Equals(SearchFieldType.CaseManager))
            {
                searchViewModel.CaseManagerResults = _context.SearchCaseManagers(searchViewModel.UserInput);

            } else if (searchViewModel.SelectedColumn.Equals(SearchFieldType.Program))
            {
                searchViewModel.ProgramResults = _context.SearchPrograms(searchViewModel.UserInput);
            } else // "All"
            {
                searchViewModel.ConsumerResults = _context.SearchConsumers(searchViewModel.UserInput);
                searchViewModel.CaseManagerResults = _context.SearchCaseManagers(searchViewModel.UserInput);
                searchViewModel.ProgramResults = _context.SearchPrograms(searchViewModel.UserInput);
            }

            return View("Index", searchViewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
