using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class CaseManager : Person
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int CProgramID { get; set; }
        public CProgram CProgram { get; set; }

        public ICollection<File> Files { get; set; }

    }
}
