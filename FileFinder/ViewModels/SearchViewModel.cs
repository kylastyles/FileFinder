using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileFinder.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileFinder.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        [Display(Name = "Search Term: ")]
        public string UserInput { get; set; }

        public string SelectedColumn { get; set; }

        public IEnumerable<SelectListItem> Columns
        { get
            {
                return Enum.GetValues(typeof(SearchFieldType)).Cast<SearchFieldType>().Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = ((int)s).ToString()
                }).ToList();
            }
        }

        public List<Consumer> ConsumerResults { get; set; }
        public List<CaseManager> CaseManagerResults { get; set; }
        public List<Models.Program> ProgramResults { get; set; }

        public SearchViewModel()
        {
            SelectedColumn = SearchFieldType.All.ToString();

        }

        

    }

}

