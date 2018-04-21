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

            //searchViewModel.Columns.Add(SearchFieldType.All);
            //searchViewModel.Columns.Add(SearchFieldType.CaseManager);
            //searchViewModel.Columns.Add(SearchFieldType.Consumer);
            //searchViewModel.Columns.Add(SearchFieldType.Program);

            return View(searchViewModel);
        }
    

        [HttpPost]
        public IActionResult Results(SearchViewModel searchViewModel)
        {

            if (searchViewModel.Column.Equals(SearchFieldType.Consumer) || searchViewModel.UserInput.Equals(""))
            {
                searchViewModel.Results = _context.FindByValue(searchViewModel.UserInput);

            }
            return View(searchViewModel);
        }


    }
}
