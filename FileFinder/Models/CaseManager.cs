using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.Models
{
    public class CaseManager : Person
    {
        public int ID { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name ="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public int ProgramID { get; set; }
        public Program Program { get; set; }

        //public ICollection<File> Files { get; set; }
        public List<File> Files { get; set; }

    }
}
