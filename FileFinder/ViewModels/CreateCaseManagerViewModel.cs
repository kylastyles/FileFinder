﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.ViewModels
{
    public class CreateCaseManagerViewModel
    {
        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Program")]
        public int ProgramID { get; set; }

        public List<SelectListItem> Programs { get; set; }
    }
}
