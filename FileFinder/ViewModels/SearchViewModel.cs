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

        public SearchFieldType SelectedColumn { get; set; }

        public List<SearchFieldType> Columns = new List<SearchFieldType>
        {
            SearchFieldType.All,
            SearchFieldType.CaseManager,
            SearchFieldType.Consumer,
            SearchFieldType.Program
        };

        //public List<Consumer> ConsumerResults = new List<Consumer>();
        //public List<CaseManager> CaseManagerResults = new List<CaseManager>();
        //public List<Models.Program> ProgramResults = new List<Models.Program>();

        public List<Consumer> ConsumerResults { get; set; }
        public List<CaseManager> CaseManagerResults { get; set; }
        public List<Models.Program> ProgramResults { get; set; }

        public SearchViewModel()
        {




        }

        

    }

}

