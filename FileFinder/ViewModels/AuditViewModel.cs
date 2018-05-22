using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FileFinder.Models;

namespace FileFinder.ViewModels
{
    public class AuditViewModel
    {
        [Required]
        [StringLength(4)]
        [Display(Name= "Fiscal Year")]
        public string FiscalYear { get; set; }

        public IEnumerable<File> FirstQuarter { get; set; }

        public IEnumerable<File> SecondQuarter { get; set; }

        public IEnumerable<File> ThirdQuarter { get; set; }

        public IEnumerable<File> FourthQuarter { get; set; }
    }
}
