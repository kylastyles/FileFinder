using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.Models
{
    public abstract class Person
    {
        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        public string FullName()
        {
            return LastName + ", " + FirstName;
        }

        public override string ToString()
        {
            return this.FullName();
        }
    }
}
