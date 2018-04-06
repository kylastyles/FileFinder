using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class CProgram
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<CaseManager> CaseManagers { get; set; }
    }
}
