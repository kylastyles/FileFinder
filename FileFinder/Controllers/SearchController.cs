using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileFinder.Data;
using FileFinder.Models;
using FileFinder.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FileFinder.Controllers
{
    public class SearchController : Controller
    {
        private readonly FileFinderContext _context;

        public SearchController(FileFinderContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    SearchViewModel searchViewModel = new SearchViewModel();

        //    return View(searchViewModel);
        //}


        [HttpPost]
        public IActionResult Results(HomeViewModel homeViewModel)
        {

            if (homeViewModel.SelectedColumn.Equals(SearchFieldType.Consumer))
            {
                homeViewModel.ConsumerResults = _context.SearchConsumers(homeViewModel.UserInput);

            }
            else if (homeViewModel.SelectedColumn.Equals(SearchFieldType.CaseManager))
            {
                homeViewModel.CaseManagerResults = _context.SearchCaseManagers(homeViewModel.UserInput);

            }
            else if (homeViewModel.SelectedColumn.Equals(SearchFieldType.Program))
            {
                homeViewModel.ProgramResults = _context.SearchPrograms(homeViewModel.UserInput);
            }
            else // "All"
            {
                homeViewModel.ConsumerResults = _context.SearchConsumers(homeViewModel.UserInput);
                homeViewModel.CaseManagerResults = _context.SearchCaseManagers(homeViewModel.UserInput);
                homeViewModel.ProgramResults = _context.SearchPrograms(homeViewModel.UserInput);
            }

            return View(homeViewModel);
        }


    }
}
