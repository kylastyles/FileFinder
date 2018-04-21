using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileFinder.Models;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        [Display(Name = "Search Term: ")]
        public string UserInput { get; set; }

        public SearchFieldType Column { get; set; }

        //public List<SearchFieldType> Columns { get; set; }

        public List<SearchFieldType> Columns = new List<SearchFieldType>
        {
            SearchFieldType.All,
            SearchFieldType.CaseManager,
            SearchFieldType.Consumer,
            SearchFieldType.Program
        };

        public List<Consumer> Results = new List<Consumer>();

        public SearchViewModel()
        {




        }

        

    }

}

