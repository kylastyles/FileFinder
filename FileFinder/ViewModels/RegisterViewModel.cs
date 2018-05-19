using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileFinder.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FileFinder.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [DataType(DataType.EmailAddress)]
        [Remote("DoesEmailExist", "Home", HttpMethod = "POST", ErrorMessage ="Email already registered. Please enter a different email.")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name ="Verify Password")]
        [Compare("Password", ErrorMessage = "Both passwords must match.")]
        public string Verify { get; set; }

    }
}
