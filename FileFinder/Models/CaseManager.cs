using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class CaseManager
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

        public int ProgramID { get; set; }
        public Program Program { get; set; }

        public IList<Consumer> Consumers { get; set; }

        public CaseManager()
        {

        }
    }
}
