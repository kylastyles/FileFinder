using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.ViewModels
{
    public class LoginViewModel
    {
        //username password

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }

    }
}
