using FileFinder.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.ViewModels
{
    public class HomeViewModel
    {
        // Search Feature
        // GET
        [Required]
        [Display(Name = "Search Term: ")]
        public string UserInput { get; set; }

        public SearchFieldType SelectedColumn { get; set; }

        public List<SearchFieldType> Columns { get; set; } = new List<SearchFieldType>
            {
                SearchFieldType.All,
                SearchFieldType.Consumer,
                SearchFieldType.CaseManager,
                SearchFieldType.Program
            };

        // POST: Search Results
        public List<Consumer> ConsumerResults { get; set; }
        public List<CaseManager> CaseManagerResults { get; set; }
        public List<Models.Program> ProgramResults { get; set; }


        // Daily Tasks Feature
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Today
        {
            get
            { return DateTime.Now; }
        }

        public IEnumerable<File> ActionFiles { get; set; }

        public IEnumerable<File> InactiveFiles { get; set; }


    //Constructor
        public HomeViewModel()
        {

        }

    }

}

