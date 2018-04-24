using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileFinder.Data;
using FileFinder.Models;
using FileFinder.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public IActionResult Index()
        {
            SearchViewModel searchViewModel = new SearchViewModel();

            return View(searchViewModel);
        }
    

        [HttpPost]
        public IActionResult Results(SearchViewModel searchViewModel)
        {
            //Populate All Results Fields
            searchViewModel.ConsumerResults = _context.SearchConsumers(searchViewModel.UserInput);
            searchViewModel.CaseManagerResults = _context.SearchCaseManagers(searchViewModel.UserInput);
            searchViewModel.ProgramResults = _context.SearchPrograms(searchViewModel.UserInput);

            //Remove fields not selected by user
            if (searchViewModel.SelectedColumn.Equals(SearchFieldType.Consumer))
            {
                searchViewModel.CaseManagerResults = null;
                searchViewModel.ProgramResults = null;
            }

            if (searchViewModel.SelectedColumn.Equals(SearchFieldType.CaseManager))
            {
                searchViewModel.ConsumerResults = null;
                searchViewModel.ProgramResults = null;
            }

            if (searchViewModel.SelectedColumn.Equals(SearchFieldType.Program))
            {
                searchViewModel.ConsumerResults = null;
                searchViewModel.CaseManagerResults = null;
            }

            return View(searchViewModel);
        }


    }
}
