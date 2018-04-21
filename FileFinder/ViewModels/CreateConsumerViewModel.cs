using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FileFinder.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileFinder.ViewModels
{
    public class CreateConsumerViewModel
    {
        public bool Active { get; set; } = true;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }

    }
}
