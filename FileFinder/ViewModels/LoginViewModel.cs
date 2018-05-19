using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FileFinder.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [MinLength(6)]
        [DataType(DataType.EmailAddress)]
        [Remote("EmailNotRegistered", "Home", HttpMethod = "POST", ErrorMessage = "Email not found. Please register.")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Remote("WrongPassword", "Home", HttpMethod = "POST", ErrorMessage = "Incorrect password.")]
        public string Password { get; set; }

    }
}
