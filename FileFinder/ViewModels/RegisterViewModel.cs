using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileFinder.Models;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [Display(Name ="Verify Password")]
        public string Verify { get; set; }

        [Display(Name = "Role")]
        public List<Role> GetRoles { get; set; } =  new List<Role>
        {
            Role.Admin,
            Role.User
        };

        public Role Role { get; set; }
    }
}
